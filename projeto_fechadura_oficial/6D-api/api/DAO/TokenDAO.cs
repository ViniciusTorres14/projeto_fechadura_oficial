using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using _6D.Models;
using _6D.Repository;

namespace _6D.DAO
{
    /// <summary>
    /// Data Access Object for user tokens.
    /// </summary>
    public class TokenDAO
    {
        private readonly MySqlConnection _connection;

        public TokenDAO()
        {
            _connection = MySqlConnectorFactory.GetConnection();
        }

        /// <summary>
        /// Removes expired tokens from the database.
        /// </summary>
        /// <param name="currentTime">The current UTC time.</param>
        /// <returns>The number of tokens removed.</returns>
        public async Task<int> RemoveExpiredTokensAsync(DateTime currentTime)
        {
            int removedCount = 0;
            try
            {
                _connection.Open();
                const string selectQuery = "SELECT * FROM db_6D_fechadura.tokens_usuario WHERE expiracao <= @currentTime";
                var selectCommand = new MySqlCommand(selectQuery, _connection);
                selectCommand.Parameters.AddWithValue("@currentTime", currentTime);

                var expiredTokens = new List<TokenUsuario>();
                using (var reader = await selectCommand.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var token = new TokenUsuario
                        {
                            TokenId = reader.GetInt32(reader.GetOrdinal("id_token")),
                            UsuarioId = reader.GetInt32(reader.GetOrdinal("id_funcionario")),
                            Token = reader.GetString(reader.GetOrdinal("token")),
                            Expiration = reader.GetDateTime(reader.GetOrdinal("expiracao")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("criado_em"))
                        };
                        expiredTokens.Add(token);
                    }
                }

                if (expiredTokens.Any())
                {
                    const string deleteQuery = "DELETE FROM db_6D_fechadura.tokens_usuario WHERE expiracao <= @currentTime";
                    var deleteCommand = new MySqlCommand(deleteQuery, _connection);
                    deleteCommand.Parameters.AddWithValue("@currentTime", currentTime);
                    removedCount = await deleteCommand.ExecuteNonQueryAsync();
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

            return removedCount;
        }

        /// <summary>
        /// Cleans up expired tokens.
        /// </summary>
        public void CleanupTokens()
        {
            RemoveExpiredTokensAsync(DateTime.UtcNow).Wait();
        }
    }
}