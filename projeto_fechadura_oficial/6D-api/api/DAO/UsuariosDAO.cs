 // Start of Selection
using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;
using System.Security.Cryptography;
using System.Text;

namespace _6D.DAO
{
    public class UsuariosDAO
    {
        private readonly MySqlConnection _connection;

        public UsuariosDAO()
        {
            _connection = MySqlConnectorFactory.GetConnection();
        }

        private static List<Usuario> ReadAll(MySqlCommand command)
        {
            var Usuarios = new List<Usuario>();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows) return Usuarios;
            while (reader.Read())
            {
                var Usuario = new Usuario
                {
                    UsuarioId = reader.GetInt32("id_funcionario"),
                    Nome = reader.GetString("nome"),
                    Email = reader.GetString("email"),
                    SenhaHash = reader.GetString("hash_senha"),
                    PinCodigo = reader.GetString("codigo_pin"),
                    RFIDTag = reader.GetString("tag_rfid")
                };
                Usuarios.Add(Usuario);
            }

            return Usuarios;
        }

        public List<Usuario> Read()
        {
            List<Usuario> Usuarios;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuarios";
                var command = new MySqlCommand(query, _connection);
                Usuarios = ReadAll(command);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return Usuarios;
        }

        public Usuario ReadById(int id)
        {
            Usuario Usuario;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuarios WHERE id_funcionario = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id", id);
                Usuario = ReadAll(command).FirstOrDefault();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return Usuario;
        }

        public List<Usuario> ReadAll(int pageNumber, int pageSize)
        {
            List<Usuario> Usuarios;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuarios ORDER BY id_funcionario ASC LIMIT @offset, @pageSize";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                Usuarios = ReadAll(command);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return Usuarios;
        }

        public int Count()
        {
            int count = 0;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM usuarios";
                var command = new MySqlCommand(query, _connection);
                count = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return count;
        }

        public void Create(Usuario Usuario)
        {
            try
            {
                _connection.Open();
                const string query = "INSERT INTO usuarios (nome, email, hash_senha, codigo_pin, tag_rfid) " +
                                     "VALUES (@Nome, @email, @hash_senha, @codigo_pin, @tag_rfid)";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@Nome", Usuario.Nome);
                command.Parameters.AddWithValue("@email", Usuario.Email);
                command.Parameters.AddWithValue("@hash_senha", Usuario.SenhaHash);
                command.Parameters.AddWithValue("@codigo_pin", Usuario.PinCodigo);
                command.Parameters.AddWithValue("@tag_rfid", Usuario.RFIDTag);
                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Update(Usuario Usuario)
        {
            try
            {
                _connection.Open();
                const string query = "UPDATE usuarios SET " +
                                     "nome = @Nome, " +
                                     "email = @email, " +
                                     "hash_senha = @hash_senha, " +
                                     "codigo_pin = @codigo_pin, " +
                                     "tag_rfid = @tag_rfid " +
                                     "WHERE id_funcionario = @id_funcionario";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@Nome", Usuario.Nome);
                command.Parameters.AddWithValue("@email", Usuario.Email);
                command.Parameters.AddWithValue("@hash_senha", !string.IsNullOrEmpty(Usuario.SenhaHash) ? HashPassword(Usuario.SenhaHash) : (object)DBNull.Value);
                command.Parameters.AddWithValue("@codigo_pin", Usuario.PinCodigo);
                command.Parameters.AddWithValue("@tag_rfid", Usuario.RFIDTag);
                command.Parameters.AddWithValue("@id_funcionario", Usuario.UsuarioId);
                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public void Delete(int id)
        {
            try
            {
                _connection.Open();
                const string query = "DELETE FROM usuarios WHERE id_funcionario = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }
        }

        public Usuario GetEmployeeByEmail(string email)
        {
            Usuario Usuario;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuarios WHERE email = @email";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@email", email);
                Usuario = ReadAll(command).FirstOrDefault();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return Usuario;
        }

        public Usuario GetEmployeeByRFIDTag(string RFIDTag)
        {
            Usuario Usuario;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuarios WHERE tag_rfid = @tag_rfid";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@tag_rfid", RFIDTag);
                Usuario = ReadAll(command).FirstOrDefault();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return Usuario;
        }

        public string GenerateUniquePinCode()
        {
            // Implement a method to generate a unique PIN code, e.g., 4-digit code
            string pin;
            do
            {
                pin = new Random().Next(1000, 9999).ToString();
            }
            while (PinCodeExists(pin));

            return pin;
        }

        private bool PinCodeExists(string pin)
        {
            bool exists = false;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM usuarios WHERE codigo_pin = @codigo_pin";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@codigo_pin", pin);
                exists = Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return exists;
        }

        public string HashPassword(string password)
        {
            // Implement a secure password hashing mechanism, e.g., SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string GenerateUniqueRFIDTag()
        {
            string RFIDTag;
            do
            {
                RFIDTag = Guid.NewGuid().ToString("N").Substring(0, 10);
            }
            while (RFIDTagExists(RFIDTag));

            return RFIDTag;
        }

        private bool RFIDTagExists(string RFIDTag)
        {
            bool exists = false;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM usuarios WHERE tag_rfid = @tag_rfid";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@tag_rfid", RFIDTag);
                exists = Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return exists;
        }

        public Usuario GetEmployeeByUsername(string username)
        {
            return Read().FirstOrDefault(e => e.Nome == username);
        }

        public Usuario GetEmployeeByPinCode(string pinCode)
        {
            Usuario usuario = null;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuarios WHERE codigo_pin = @codigo_pin";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@codigo_pin", pinCode);
                usuario = ReadAll(command).FirstOrDefault();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                _connection.Close();
            }

            return usuario;
        }
    }
}
