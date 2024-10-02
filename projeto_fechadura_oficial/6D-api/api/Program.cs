using _6D.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.Services.ConfigureServices(builder.Configuration);

// Build the application
var app = builder.Build();

// Configure Middleware Pipeline
app.ConfigureMiddleware();

// Run the application
app.Run();