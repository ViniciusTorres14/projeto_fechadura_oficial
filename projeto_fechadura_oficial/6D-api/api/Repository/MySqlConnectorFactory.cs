using MySql.Data.MySqlClient;

namespace _6D.Repository
{
    public class MySqlConnectorFactory
    {
        public static MySqlConnection GetConnection()
        {
            const string connectionString = "server=localhost;" +
                                            "port=3306;" +
                                            "database=db_6D_fechadura;" +
                                            "uid=root;" +
                                            "pwd=root;";
            return new MySqlConnection(connectionString);
        }
    }
}