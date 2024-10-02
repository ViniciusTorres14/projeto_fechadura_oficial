using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;

namespace _6D.DAO
{
    public class SalasDAO
    {
        private readonly MySqlConnection _connection;

        public SalasDAO()
        {
            _connection = MySqlConnectorFactory.GetConnection();
        }

        private static List<Sala> ReadAll(MySqlCommand command)
        {
            var Salas = new List<Sala>();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows) return Salas;
            while (reader.Read())
            {
                var Sala = new Sala
                {
                    SalaId = reader.GetInt32("id_sala"),
                    Nome = reader.GetString("nome"),
                    Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString("descricao"),
                    Status = reader.GetBoolean("status"),
                    Imagem = reader.IsDBNull(reader.GetOrdinal("imagem")) ? null : (byte[])reader["imagem"],
                    OcupadoPorUsuarioId = reader.IsDBNull(reader.GetOrdinal("ocupado_por_funcionario_id")) ? (int?)null : reader.GetInt32("ocupado_por_funcionario_id")
                };
                Salas.Add(Sala);
            }

            return Salas;
        }

        public List<Sala> Read()
        {
            List<Sala> Salas;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM salas";
                var command = new MySqlCommand(query, _connection);
                Salas = ReadAll(command);
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

            return Salas;
        }

        public Sala ReadById(int id)
        {
            Sala Sala;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM salas WHERE id_sala = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id", id);
                Sala = ReadAll(command).FirstOrDefault();
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

            return Sala;
        }

        public List<Sala> ReadAll(int pageNumber, int pageSize)
        {
            List<Sala> Salas;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM salas ORDER BY id_sala ASC LIMIT @offset, @pageSize";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                Salas = ReadAll(command);
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

            return Salas;
        }

        public int Count()
        {
            int count = 0;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM salas";
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

        public void Create(Sala Sala)
        {
            try
            {
                _connection.Open();
                const string query = "INSERT INTO salas (nome, descricao, status, imagem, ocupado_por_funcionario_id) " +
                                     "VALUES (@Nome, @Descricao, @Status, @Imagem, @ocupado_por_funcionario_id)";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@Nome", Sala.Nome);
                command.Parameters.AddWithValue("@Descricao", (object)Sala.Descricao ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", Sala.Status);
                command.Parameters.AddWithValue("@Imagem", (object)Sala.Imagem ?? DBNull.Value);
                command.Parameters.AddWithValue("@ocupado_por_funcionario_id", (object)Sala.OcupadoPorUsuarioId ?? DBNull.Value);
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

        public void Update(Sala Sala)
        {
            try
            {
                _connection.Open();
                const string query = "UPDATE salas SET " +
                                     "nome = @Nome, " +
                                     "descricao = @Descricao, " +
                                     "status = @Status, " +
                                     "imagem = @Imagem, " +
                                     "ocupado_por_funcionario_id = @ocupado_por_funcionario_id " +
                                     "WHERE id_sala = @room_id";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@Nome", Sala.Nome);
                command.Parameters.AddWithValue("@Descricao", (object)Sala.Descricao ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", Sala.Status);
                command.Parameters.AddWithValue("@Imagem", (object)Sala.Imagem ?? DBNull.Value);
                command.Parameters.AddWithValue("@ocupado_por_funcionario_id", (object)Sala.OcupadoPorUsuarioId ?? DBNull.Value);
                command.Parameters.AddWithValue("@room_id", Sala.SalaId);
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
                const string query = "DELETE FROM salas WHERE id_sala = @id";
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

        public string GetEmployeeName(int? UsuarioId)
        {
            if (UsuarioId == null)
                return null;

            string Nome = null;
            try
            {
                _connection.Open();
                const string query = "SELECT nome FROM usuarios WHERE id_funcionario = @employee_id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@employee_id", UsuarioId.Value);
                using var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Nome = reader.GetString("nome");
                }
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

            return Nome;
        }
    }
}
