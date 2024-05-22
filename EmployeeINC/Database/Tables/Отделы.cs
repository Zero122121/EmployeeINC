using System;
using System.Collections.Generic;

namespace EmployeeINC.Database.Tables
{
    public class Отделы : Table
    {
        public int ID_Отдела;
        public string Name;

        public Отделы(int id, string name)
        {
            ID_Отдела = id;
            Name = name;
        }

        public Отделы() { }
        
        public override Table ConvertToTable(Dictionary<string, object> value)
        {
            value.TryGetValue("ID_Отдела", out var idObject);
            value.TryGetValue("Name", out var nameObject);
            int id = int.Parse(idObject.ToString());
            string name = nameObject.ToString();
            return new Отделы(id, name);
        }

        public override Table[] ConvertToTables(List<Dictionary<string, object>> value)
        {
            var result = new Отделы[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                var dictionary = value[i];
                result[i] = (Отделы)ConvertToTable(dictionary);
            }

            return result;
        }
    }
}