using Npgsql;
using System.Data;

namespace DB
{
    public class SqlExecutor
    {
        public static IEnumerable<T> ExecuteSql<T>(string sql, Func<IDataReader, T> predicate)
        {
            var conn = Connect();

            var collection = Enumerable.Empty<T>();

            using (var command = conn.CreateCommand())
            {
                command.CommandText = sql;
                using (IDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var result = predicate(dataReader);
                        collection = collection.Append(result);
                    }
                }
            }

            return collection;
        }

        public static NpgsqlConnection Connect(string connectionString = "")
        {
            connectionString = !string.IsNullOrEmpty(connectionString) ? connectionString : "Host=postgres;Port=5432;Database=SmartLinkD;Username=puser;Password=111";

            NpgsqlConnection conn = null;

            try
            {
                conn = new NpgsqlConnection(connectionString);
                conn.Open();
            }
            catch (Exception e)
            {
                throw new Exception("Error connecting to the database", e);
            }
            return conn;
        }
    }
}
