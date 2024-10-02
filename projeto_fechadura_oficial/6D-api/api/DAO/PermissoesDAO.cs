using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;

namespace _6D.DAO
{
    public class PermissoesDAO
    {
        private readonly MySqlConnection _connection;

        public PermissoesDAO()
        {
            _connection = MySqlConnectorFactory.GetConnection();
        }

        private static List<Permissao> ReadAll(MySqlCommand command)
        {
            var Permissoes = new List<Permissao>();

            using var reader = command.ExecuteReader();
            if (!reader.HasRows) return Permissoes;
            while (reader.Read())
            {
                var Permissao = new Permissao
                {
                    PermissaoId = reader.GetInt32("id_permissao"),
                    PermissionKey = reader.GetString("chave_permissao"),
                    Descricao = reader.IsDBNull(reader.GetOrdinal("descricao")) ? null : reader.GetString("descricao")
                };
                Permissoes.Add(Permissao);
            }

            return Permissoes;
        }

        public List<Permissao> Read()
        {
            List<Permissao> Permissoes;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM permissoes";
                var command = new MySqlCommand(query, _connection);
                Permissoes = ReadAll(command);
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

            return Permissoes;
        }

        public Permissao ReadById(int id)
        {
            Permissao Permissao;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM permissoes WHERE id_permissao = @id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@id", id);
                Permissao = ReadAll(command).FirstOrDefault();
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

            return Permissao;
        }

        public List<Permissao> ReadAll(int pageNumber, int pageSize)
        {
            List<Permissao> Permissoes;
            try
            {
                _connection.Open();
                const string query = "SELECT * FROM permissoes ORDER BY id_permissao ASC LIMIT @offset, @pageSize";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                Permissoes = ReadAll(command);
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

            return Permissoes;
        }

        public int Count()
        {
            int count = 0;
            try
            {
                _connection.Open();
                const string query = "SELECT COUNT(*) FROM permissoes";
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

        public void Create(Permissao Permissao)
        {
            try
            {
                _connection.Open();
                const string query = "INSERT INTO permissoes (chave_permissao, descricao) " +
                                     "VALUES (@chave_permissao, @descricao)";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@chave_permissao", Permissao.PermissionKey);
                command.Parameters.AddWithValue("@descricao", (object)Permissao.Descricao ?? DBNull.Value);
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

        public void Update(Permissao Permissao)
        {
            try
            {
                _connection.Open();
                const string query = "UPDATE permissoes SET " +
                                     "chave_permissao = @chave_permissao, " +
                                     "descricao = @descricao " +
                                     "WHERE id_permissao = @id_permissao";

                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@chave_permissao", Permissao.PermissionKey);
                command.Parameters.AddWithValue("@descricao", (object)Permissao.Descricao ?? DBNull.Value);
                command.Parameters.AddWithValue("@id_permissao", Permissao.PermissaoId);
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
                const string query = "DELETE FROM permissoes WHERE id_permissao = @id";
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

        public List<Permissao> ReadByRoleId(int CargoId, int pageNumber, int pageSize)
        {
            List<Permissao> Permissoes;
            try
            {
                _connection.Open();
                const string query = @"SELECT p.*
                                       FROM permissoes p
                                       INNER JOIN role_permissions rp ON p.id_permissao = rp.permission_id
                                       WHERE rp.role_id = @role_id
                                       ORDER BY p.id_permissao ASC
                                       LIMIT @offset, @pageSize";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@role_id", CargoId);
                command.Parameters.AddWithValue("@offset", (pageNumber - 1) * pageSize);
                command.Parameters.AddWithValue("@pageSize", pageSize);
                Permissoes = ReadAll(command);
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

            return Permissoes;
        }

        public int CountByRoleId(int CargoId)
        {
            int count = 0;
            try
            {
                _connection.Open();
                const string query = @"SELECT COUNT(*) 
                                       FROM permissoes p
                                       INNER JOIN role_permissions rp ON p.id_permissao = rp.permission_id
                                       WHERE rp.role_id = @role_id";
                var command = new MySqlCommand(query, _connection);
                command.Parameters.AddWithValue("@role_id", CargoId);
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
    }
}
