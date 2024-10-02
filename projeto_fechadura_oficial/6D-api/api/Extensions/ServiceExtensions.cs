using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using _6D.Models;
using _6D.Services;
using _6D.DAO;

namespace _6D.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure JWT Settings
            services.Configure<ConfiguracoesJwt>(configuration.GetSection("Jwt"));

            // Register DAOs
            services.AddScoped<RegistroDeAcessoDAO>();
            services.AddScoped<AutenticacaoDAO>();
            services.AddScoped<UsuarioCargoDAO>();
            services.AddScoped<UsuarioSalaAcessoDAO>();
            services.AddScoped<UsuariosDAO>();
            services.AddScoped<PermissoesDAO>();
            services.AddScoped<CargosDAO>();
            services.AddScoped<SalasDAO>();
            services.AddScoped<CargoPermissoesDAO>();
            services.AddScoped<TokenDAO>();

            // Controllers
            services.AddControllers();

            // Swagger/OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "6D API", Version = "v1" });

                // Add JWT Authentication to Swagger
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    Description = "Enter JWT Bearer token **_only_**",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });

            // Configure CORS for all origins (*)
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins("*")
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            // Add Health Checks
            services.AddHealthChecks();

            // Add Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = configuration.GetSection("Jwt").Get<ConfiguracoesJwt>();
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Add the JwtService
            services.AddScoped<IJwtService, JwtService>();

            // Add TokenCleanupService
            services.AddSingleton<IHostedService, TokenCleanupService>();
        }
    }
}