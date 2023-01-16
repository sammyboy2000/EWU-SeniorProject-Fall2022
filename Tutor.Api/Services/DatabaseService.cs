using MySql.Data.MySqlClient;
using System.Data;
using Tutor.Api.Services;

namespace Tutor.api.Services
{
    public class DatabaseService
    {
        MySqlConnection connection;
        public DatabaseService()
        {
            try
            {
                //Connection String
                connection = new("server=localhost;uid=root;password=example;database=tutor_db");
                connection.Open();
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
            }
        }
        internal IEnumerable<String> GetSchema()
        {
            
            List<String> schemas = new();
            try
            {
                connection.Open();

                DataTable table = connection.GetSchema("MetaDataCollections");
                schemas = ListData(table);

                connection.Close();
                return schemas;
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
            }
            return new List<String>(){"error"};
        }
        internal IEnumerable<Class> GetClasses()
        {
            IEnumerable<Class> classes = new List<Class>();
            connection.Open();
            using (var cmd = new MySqlCommand("SELECT * FROM Classes", connection))
            using (var reader = cmd.ExecuteReader())
                while (reader.Read())
                    Console.WriteLine(reader.GetString(0));

            return classes;
        }
        private static List<String> ListData(DataTable table)
        {
            List<String> data = new();
            foreach (System.Data.DataRow row in table.Rows)
            {
                foreach (System.Data.DataColumn col in table.Columns)
                {
                    data.Add(col.ColumnName + " " + row[col] + "\n");
                }
                data.Add("============================");
            }
            return data;
        }
    }
}