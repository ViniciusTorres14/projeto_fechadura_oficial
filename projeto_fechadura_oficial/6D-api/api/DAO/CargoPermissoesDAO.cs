using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;

namespace _6D.DAO
{
    public class CargoPermissoesDAO
    {
        private readonly MySqlConnection _connection;

        public CargoPermissoesDAO()
        {
            _connection = MySqlConnectorFactory.GetConnection();
        }

        private static List<CargoPermissao> ReadAll(MySqlCommand command)
        {
            var CargoPermissoes = new List<CargoPermissao>();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows) return CargoPermissoes;
            while (reader.Read())
            {
                var rp = new CargoPermissao
                {
                    CargoPermissaoId = reader.GetInt32("id_permissao_cargo"),
                    CargoId = reader.GetInt32("id_cargo"),
                    PermissaoId = reader.GetInt32("id_permissao")
                };
                CargoPermissoes.Add(rp);
            }

            return CargoPermissoes;
        }

        public List<CargoPermissao> Read()
        {
            List<CargoPermissao> CargoPermissoes;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM cargo_permissoes";
                var command = new MySqlCommand(query, _connection);
                CargoPermissoes = ReadAll(command);
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

            return CargoPermissoes;
        }

        public CargoPermissao ReadById(int id)
        {
            CargoPermissao rp;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM cargo_permissoes WHERE id_permissao_cargo = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id", id);
                rp = ReadAll(command).FirstOrDefault();
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

            return rp;
        }

        public List<CargoPermissao> ReadAll(int pageNumber, int pageSize)
        {
            List<CargoPermissao> CargoPermissoes;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM cargo_permissoes ORDER BY id_permissao_cargo ASC LIMIT @offset, @pageSize";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                CargoPermissoes = ReadAll(command);
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

            return CargoPermissoes;
        }

        public int Count()
        {
            int count = 0;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM cargo_permissoes";
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

        public void Create(CargoPermissao rp)
        {
            try
            {
                _connection.Open();
                const string query = "INSERT INTO cargo_permissoes (id_cargo, id_permissao) " +
                                     "VALUES (@id_cargo, @id_permissao)";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_cargo", rp.CargoId);
                command.Parameters.AddWithValue("@id_permissao", rp.PermissaoId);
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

        public void Update(CargoPermissao rp)
        {
            try
            {
                _connection.Open();
                const string query = "UPDATE cargo_permissoes SET " +
                                     "id_cargo = @id_cargo, " +
                                     "id_permissao = @id_permissao " +
                                     "WHERE id_permissao_cargo = @id_permissao_cargo";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_cargo", rp.CargoId);
                command.Parameters.AddWithValue("@id_permissao", rp.PermissaoId);
                command.Parameters.AddWithValue("@id_permissao_cargo", rp.CargoPermissaoId);
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
                const string query = "DELETE FROM cargo_permissoes WHERE id_permissao_cargo = @id";
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

        public List<CargoPermissao> ReadByRoleId(int CargoId)
        {
            List<CargoPermissao> CargoPermissoes;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM cargo_permissoes WHERE id_cargo = @id_cargo";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_cargo", CargoId);
                CargoPermissoes = ReadAll(command);
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

            return CargoPermissoes;
        }

        public List<CargoPermissao> ReadByPermissionId(int PermissaoId)
        {
            List<CargoPermissao> CargoPermissoes;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM cargo_permissoes WHERE id_permissao = @id_permissao";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id_permissao", PermissaoId);
                CargoPermissoes = ReadAll(command);
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

            return CargoPermissoes;
        }
    }
}
