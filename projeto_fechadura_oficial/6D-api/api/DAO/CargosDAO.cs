using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;

namespace _6D.DAO
{
    public class CargosDAO
    {
        private readonly MySqlConnection _connection;

        public CargosDAO()
        {
            _connection = MySqlConnectorFactory.GetConnection();
        }

        private static List<Cargo> ReadAll(MySqlCommand command)
        {
            var Cargos = new List<Cargo>();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows) return Cargos;
            while (reader.Read())
            {
                var Cargo = new Cargo
                {
                    CargoId = reader.GetInt32("id_cargo"),
                    RoleName = reader.GetString("nome_cargo"),
                    Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString("descricao")
                    // Assuming CargoPermissoes is loaded separately
                };
                Cargos.Add(Cargo);
            }

            return Cargos;
        }

        public List<Cargo> Read()
        {
            List<Cargo> Cargos;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM cargos";
                var command = new MySqlCommand(query, _connection);
                Cargos = ReadAll(command);
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

            return Cargos;
        }

        public Cargo ReadById(int id)
        {
            Cargo Cargo;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM cargos WHERE id_cargo = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id", id);
                Cargo = ReadAll(command).FirstOrDefault();
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

            return Cargo;
        }

        public List<Cargo> ReadAll(int pageNumber, int pageSize)
        {
            List<Cargo> Cargos;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM cargos ORDER BY id_cargo ASC LIMIT @offset, @pageSize";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                Cargos = ReadAll(command);
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

            return Cargos;
        }

        public int Count()
        {
            int count = 0;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM cargos";
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

        public void Create(Cargo Cargo)
        {
            try
            {
                _connection.Open();
                const string query = "INSERT INTO cargos (nome_cargo, descricao) " +
                                     "VALUES (@nome_cargo, @descricao)";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@nome_cargo", Cargo.RoleName);
                command.Parameters.AddWithValue("@descricao", (object)Cargo.Descricao ?? DBNull.Value);
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

        public void Update(Cargo Cargo)
        {
            try
            {
                _connection.Open();
                const string query = "UPDATE cargos SET " +
                                     "nome_cargo = @nome_cargo, " +
                                     "descricao = @descricao " +
                                     "WHERE id_cargo = @id_cargo";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@nome_cargo", Cargo.RoleName);
                command.Parameters.AddWithValue("@descricao", (object)Cargo.Descricao ?? DBNull.Value);
                command.Parameters.AddWithValue("@id_cargo", Cargo.CargoId);
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
                const string query = "DELETE FROM cargos WHERE id_cargo = @id";
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

        // Additional Methods
        public Cargo GetRoleByName(string roleName)
        {
            Cargo Cargo;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM cargos WHERE nome_cargo = @nome_cargo";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@nome_cargo", roleName);
                Cargo = ReadAll(command).FirstOrDefault();
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

            return Cargo;
        }

        public List<Cargo> GetRolesByEmployeeId(int UsuarioId)
        {
            List<Cargo> Cargos = new List<Cargo>();
            try
            {
                _connection.Open();
                const string query = @"SELECT r.*
                                       FROM cargos r
                                       INNER JOIN employee_roles er ON r.id_cargo = er.role_id
                                       WHERE er.employee_id = @employee_id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@employee_id", UsuarioId);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var Cargo = new Cargo
                    {
                        CargoId = reader.GetInt32("id_cargo"),
                        RoleName = reader.GetString("nome_cargo"),
                        Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString("descricao")
                    };
                    Cargos.Add(Cargo);
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

            return Cargos;
        }
    }
}
