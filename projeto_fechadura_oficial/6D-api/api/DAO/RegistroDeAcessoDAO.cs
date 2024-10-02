using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;

namespace _6D.DAO
{
    public class RegistroDeAcessoDAO
    {
        private readonly MySqlConnection _connection;

        public RegistroDeAcessoDAO()
        {
            _connection = MySqlConnectorFactory.GetConnection();
        }

        private static List<RegistrosDeAcesso> ReadAll(MySqlCommand command)
        {
            var logs = new List<RegistrosDeAcesso>();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows) return logs;
            while (reader.Read())
            {
                var log = new RegistrosDeAcesso
                {
                    RegistroId = reader.GetInt32("id_registro"),
                    UsuarioId = reader.IsDBNull(reader.GetOrdinal("id_funcionario")) ? (int?)null : reader.GetInt32("id_funcionario"),
                    SalaId = reader.IsDBNull(reader.GetOrdinal("id_sala")) ? (int?)null : reader.GetInt32("id_sala"),
                    AccessTime = reader.IsDBNull(reader.GetOrdinal("tempo_acesso")) ? (DateTime?)null : reader.GetDateTime("tempo_acesso"),
                    AccessGranted = reader.IsDBNull(reader.GetOrdinal("acesso_concedido")) ? (bool?)null : reader.GetBoolean("acesso_concedido")
                };
                logs.Add(log);
            }

            return logs;
        }

        public List<RegistrosDeAcesso> Read()
        {
            List<RegistrosDeAcesso> logs;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM registros_de_acesso";
                var command = new MySqlCommand(query, _connection);
                logs = ReadAll(command);
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

            return logs;
        }

        public RegistrosDeAcesso ReadById(int id)
        {
            RegistrosDeAcesso log;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM registros_de_acesso WHERE id_registro = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id", id);
                log = ReadAll(command).FirstOrDefault();
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

            return log;
        }

        public List<RegistrosDeAcesso> ReadAll(int pageNumber, int pageSize)
        {
            List<RegistrosDeAcesso> logs;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM registros_de_acesso ORDER BY tempo_acesso DESC LIMIT @offset, @pageSize";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                logs = ReadAll(command);
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

            return logs;
        }

        public int Count()
        {
            int count = 0;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM registros_de_acesso";
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

        public void Create(RegistrosDeAcesso log)
        {
            try
            {
                _connection.Open();
                const string query = "INSERT INTO registros_de_acesso (id_funcionario, id_sala, tempo_acesso, acesso_concedido) " +
                                     "VALUES (@id_funcionario, @id_sala, @tempo_acesso, @acesso_concedido)";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_funcionario", (object)log.UsuarioId ?? DBNull.Value);
                command.Parameters.AddWithValue("@id_sala", (object)log.SalaId ?? DBNull.Value);
                command.Parameters.AddWithValue("@tempo_acesso", (object)log.AccessTime ?? DBNull.Value);
                command.Parameters.AddWithValue("@acesso_concedido", (object)log.AccessGranted ?? DBNull.Value);
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

        public void Update(RegistrosDeAcesso log)
        {
            try
            {
                _connection.Open();
                const string query = "UPDATE registros_de_acesso SET " +
                                     "id_funcionario = @id_funcionario, " +
                                     "id_sala = @id_sala, " +
                                     "tempo_acesso = @tempo_acesso, " +
                                     "acesso_concedido = @acesso_concedido " +
                                     "WHERE id_registro = @id_registro";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_funcionario", (object)log.UsuarioId ?? DBNull.Value);
                command.Parameters.AddWithValue("@id_sala", (object)log.SalaId ?? DBNull.Value);
                command.Parameters.AddWithValue("@tempo_acesso", (object)log.AccessTime ?? DBNull.Value);
                command.Parameters.AddWithValue("@acesso_concedido", (object)log.AccessGranted ?? DBNull.Value);
                command.Parameters.AddWithValue("@id_registro", log.RegistroId);
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
                const string query = "DELETE FROM registros_de_acesso WHERE id_registro = @id";
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

        public async Task CreateAsync(RegistrosDeAcesso registro)
        {
            try
            {
                _connection.Open();
                const string query = "INSERT INTO registros_de_acesso (id_funcionario, id_sala, tempo_acesso, acesso_concedido) " +
                                    "VALUES (@id_funcionario, @id_sala, @tempo_acesso, @acesso_concedido)";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_funcionario", (object?)registro.UsuarioId ?? DBNull.Value);
                command.Parameters.AddWithValue("@id_sala", (object?)registro.SalaId ?? DBNull.Value);
                command.Parameters.AddWithValue("@tempo_acesso", (object?)registro.AccessTime ?? DBNull.Value);
                command.Parameters.AddWithValue("@acesso_concedido", (object?)registro.AccessGranted ?? DBNull.Value);
                await command.ExecuteNonQueryAsync();
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
    }
}
