using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;

namespace _6D.DAO
{
    public class UsuarioSalaAcessoDAO
    {
        private readonly MySqlConnection _connection;

        public UsuarioSalaAcessoDAO()
        {
            _connection = MySqlConnectorFactory.GetConnection();
        }

        private static List<UsuarioSalaAcesso> ReadAll(MySqlCommand command)
        {
            var accesses = new List<UsuarioSalaAcesso>();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows) return accesses;
            while (reader.Read())
            {
                var access = new UsuarioSalaAcesso
                {
                    AcessoId = reader.GetInt32("access_id"),
                    UsuarioId = reader.IsDBNull(reader.GetOrdinal("employee_id")) ? (int?)null : reader.GetInt32("employee_id"),
                    SalaId = reader.IsDBNull(reader.GetOrdinal("room_id")) ? (int?)null : reader.GetInt32("room_id")
                };
                accesses.Add(access);
            }

            return accesses;
        }

        public List<UsuarioSalaAcesso> Read()
        {
            List<UsuarioSalaAcesso> accesses;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM employee_room_access";
                var command = new MySqlCommand(query, _connection);
                accesses = ReadAll(command);
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

            return accesses;
        }

        public UsuarioSalaAcesso ReadById(int id)
        {
            UsuarioSalaAcesso access;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM employee_room_access WHERE access_id = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id", id);
                access = ReadAll(command).FirstOrDefault();
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

            return access;
        }

        public List<UsuarioSalaAcesso> ReadAll(int pageNumber, int pageSize)
        {
            List<UsuarioSalaAcesso> accesses;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM employee_room_access ORDER BY access_id ASC LIMIT @offset, @pageSize";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                accesses = ReadAll(command);
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

            return accesses;
        }

        public int Count()
        {
            int count = 0;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM employee_room_access";
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

        public void Create(UsuarioSalaAcesso access)
        {
            try
            {
                _connection.Open();
                const string query = "INSERT INTO employee_room_access (employee_id, room_id) " +
                                     "VALUES (@employee_id, @room_id)";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@employee_id", (object)access.UsuarioId ?? DBNull.Value);
                command.Parameters.AddWithValue("@room_id", (object)access.SalaId ?? DBNull.Value);
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

        public void Update(UsuarioSalaAcesso access)
        {
            try
            {
                _connection.Open();
                const string query = "UPDATE employee_room_access SET " +
                                     "employee_id = @employee_id, " +
                                     "room_id = @room_id " +
                                     "WHERE access_id = @access_id";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@employee_id", (object)access.UsuarioId ?? DBNull.Value);
                command.Parameters.AddWithValue("@room_id", (object)access.SalaId ?? DBNull.Value);
                command.Parameters.AddWithValue("@access_id", access.AcessoId);
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
                const string query = "DELETE FROM employee_room_access WHERE access_id = @id";
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

        public List<UsuarioSalaAcesso> ReadByEmployeeId(int UsuarioId)
        {
            List<UsuarioSalaAcesso> accesses;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM employee_room_access WHERE employee_id = @employee_id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@employee_id", UsuarioId);
                accesses = ReadAll(command);
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

            return accesses;
        }

        public List<UsuarioSalaAcesso> ReadByRoomId(int SalaId)
        {
            List<UsuarioSalaAcesso> accesses;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM employee_room_access WHERE room_id = @room_id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@room_id", SalaId);
                accesses = ReadAll(command);
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

            return accesses;
        }

        public bool UserHasAccessToRoom(int userId, int roomId)
        {
            bool hasAccess = false;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM employee_room_access WHERE employee_id = @employee_id AND room_id = @room_id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@employee_id", userId);
                command.Parameters.AddWithValue("@room_id", roomId);
                hasAccess = Convert.ToInt32(command.ExecuteScalar()) > 0;
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

            return hasAccess;
        }
    }
}
