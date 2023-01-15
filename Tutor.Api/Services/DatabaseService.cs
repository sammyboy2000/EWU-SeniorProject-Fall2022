using MySqlConnector;
using Tutor.Api.Services;

namespace Tutor.api.Services
{
    public class DatabaseService
    {
        internal IEnumerable<Class> GetClasses(MySqlConnection sqlConnection)
        {
            IEnumerable<Class> classes = new List<Class>();
            sqlConnection.Open();
            using (var cmd = new MySqlCommand("SELECT * FROM Classes", sqlConnection))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    Console.WriteLine(reader.GetString(0));

            return classes;
        }

    }
}