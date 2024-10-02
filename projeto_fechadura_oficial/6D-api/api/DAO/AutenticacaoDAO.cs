using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using _6D.Services;

namespace _6D.DAO
{
    public class AutenticacaoDAO
    {
        private readonly UsuariosDAO _userRepo;
        private readonly ConfiguracoesJwt _jwtSettings;
        private readonly IConfiguration _configuration;
        private readonly IJwtService _jwtService;

        public AutenticacaoDAO(UsuariosDAO userRepo, IOptions<ConfiguracoesJwt> ConfiguracoesJwt, IConfiguration configuration, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _jwtSettings = ConfiguracoesJwt.Value;
            _configuration = configuration;
            _jwtService = jwtService;
        }

        public string Register(RegisterDto registerDto)
        {

            string passwordHashed = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);
            Console.WriteLine(passwordHashed);
            Console.WriteLine(registerDto.Password);
            var usuario = new Usuario
            {
                Nome = registerDto.Username,
                Email = registerDto.Username,
                SenhaHash = passwordHashed,
                PinCodigo = _userRepo.GenerateUniquePinCode(),
                RFIDTag = _userRepo.GenerateUniqueRFIDTag()
            };

            _userRepo.Create(usuario);

            // Use the plain password to login and generate token
            return _jwtService.GenerateToken(usuario);
        }

        public string Login(string username, string password)
        {
            var usuario = _userRepo.GetEmployeeByUsername(username);
            if (usuario == null)
                return null;

            Console.WriteLine(usuario.SenhaHash);
            Console.WriteLine(password);
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, usuario.SenhaHash);
            if (!isPasswordValid)
                return null;

            return _jwtService.GenerateToken(usuario);
        }

        public bool UserExists(string username)
        {
            return _userRepo.GetEmployeeByUsername(username) != null;
        }

        public Usuario GetUserById(int id)
        {
            return _userRepo.ReadById(id);
        }

        public class RegisterDto
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
