using System.Collections.Generic;
using System.Data.SQLite;

namespace DB
{
    public static class Database
    {
        private static readonly SQLiteConnection Connection = new SQLiteConnection(CONNECTION_STRING);

        // private const string CONNECTION_STRING = @"Data Source=C:\Kimicu\Programming\DesktopApps\KimicuHelper\KimicuHelper\Databases\project_database;Version=3;";
        private const string CONNECTION_STRING = @"Data Source=|DataDirectory|\Database\database;";
    
        public static void Connect() => Connection.Open();
        public static void Disconnect() => Connection.Close();

        public static void Query(string query) => new SQLiteCommand(query, Connection).ExecuteReader();

        public static List<Dictionary<string, object>> ExecuteQuery(string query)
        {
            var results = new List<Dictionary<string, object>>();
            using SQLiteCommand command = new SQLiteCommand(query, Connection);
            using SQLiteDataReader reader = command.ExecuteReader();
  
            while (reader.Read())
            {
                var result = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    result.Add(reader.GetName(i), reader.GetValue(i));
                }
                results.Add(result);
            }

            return results;
        }
    }
}