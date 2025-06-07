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

        public static void ExecuteInsert(string insertSql, params NpgsqlParameter[] parameters)
        {
            var conn = Connect();
           
            using (var cmd = new NpgsqlCommand(insertSql, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }
                int affectedRows = cmd.ExecuteNonQuery();
            }            
        }

        public static NpgsqlConnection Connect(string connectionString = "")
        {
            connectionString = !string.IsNullOrEmpty(connectionString) ? connectionString : Environment.GetEnvironmentVariable("CONNECTION_STRING");
            //connectionString =  "Host=localhost;Port=5433;Database=SmartLinkNew;Username=puser;Password=111;Timeout=500; CommandTimeout=400;MaxPoolSize=1024;";

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
