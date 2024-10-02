using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;

namespace _6D.DAO
{
    public class UsuarioCargoDAO
    {
        private readonly MySqlConnection _connection;

        public UsuarioCargoDAO()
        {
            _connection = MySqlConnectorFactory.GetConnection();
        }

        private static List<UsuarioCargo> ReadAll(MySqlCommand command)
        {
            var employeeRoles = new List<UsuarioCargo>();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows) return employeeRoles;
            while (reader.Read())
            {
                var UsuarioCargo = new UsuarioCargo
                {
                    EmployeeRoleId = reader.GetInt32("id_funcionario_cargo"),
                    UsuarioId = reader.IsDBNull(reader.GetOrdinal("id_funcionario")) ? (int?)null : reader.GetInt32("id_funcionario"),
                    CargoId = reader.IsDBNull(reader.GetOrdinal("id_cargo")) ? (int?)null : reader.GetInt32("id_cargo")
                };
                employeeRoles.Add(UsuarioCargo);
            }

            return employeeRoles;
        }

        public List<UsuarioCargo> Read()
        {
            List<UsuarioCargo> employeeRoles;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuario_cargos";
                var command = new MySqlCommand(query, _connection);
                employeeRoles = ReadAll(command);
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

            return employeeRoles;
        }

        public UsuarioCargo ReadById(int id)
        {
            UsuarioCargo UsuarioCargo;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuario_cargos WHERE id_funcionario_cargo = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id", id);
                UsuarioCargo = ReadAll(command).FirstOrDefault();
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

            return UsuarioCargo;
        }

        public List<UsuarioCargo> ReadAll(int pageNumber, int pageSize)
        {
            List<UsuarioCargo> employeeRoles;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuario_cargos ORDER BY id_funcionario_cargo ASC LIMIT @offset, @pageSize";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                employeeRoles = ReadAll(command);
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

            return employeeRoles;
        }

        public int Count()
        {
            int count = 0;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM usuario_cargos";
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

        public void Create(UsuarioCargo UsuarioCargo)
        {
            try
            {
                _connection.Open();
                const string query = "INSERT INTO usuario_cargos (id_funcionario, id_cargo) " +
                                     "VALUES (@id_funcionario, @id_cargo)";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_funcionario", (object)UsuarioCargo.UsuarioId ?? DBNull.Value);
                command.Parameters.AddWithValue("@id_cargo", (object)UsuarioCargo.CargoId ?? DBNull.Value);
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

        public void Update(UsuarioCargo UsuarioCargo)
        {
            try
            {
                _connection.Open();
                const string query = "UPDATE usuario_cargos SET " +
                                     "id_funcionario = @id_funcionario, " +
                                     "id_cargo = @id_cargo " +
                                     "WHERE id_funcionario_cargo = @id_funcionario_cargo";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_funcionario", (object)UsuarioCargo.UsuarioId ?? DBNull.Value);
                command.Parameters.AddWithValue("@id_cargo", (object)UsuarioCargo.CargoId ?? DBNull.Value);
                command.Parameters.AddWithValue("@id_funcionario_cargo", UsuarioCargo.EmployeeRoleId);
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
                const string query = "DELETE FROM usuario_cargos WHERE id_funcionario_cargo = @id";
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

        public List<UsuarioCargo> ReadByEmployeeId(int UsuarioId)
        {
            List<UsuarioCargo> employeeRoles;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuario_cargos WHERE id_funcionario = @id_funcionario";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_funcionario", UsuarioId);
                employeeRoles = ReadAll(command);
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

            return employeeRoles;
        }

        public List<UsuarioCargo> ReadByRoleId(int CargoId)
        {
            List<UsuarioCargo> employeeRoles;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM usuario_cargos WHERE id_cargo = @id_cargo";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_cargo", CargoId);
                employeeRoles = ReadAll(command);
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

            return employeeRoles;
        }
    }
}
