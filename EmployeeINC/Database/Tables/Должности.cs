using System;
using System.Collections.Generic;

namespace EmployeeINC.Database.Tables
{
    public class Должности : Table
    {
        public int ID_Должности;
        public string Наименование;
        public int Ставка;

        public Должности(int id, string name, int ставка)
        {
            ID_Должности = id;
            Наименование = name;
            Ставка = ставка;
        }

        public Должности() { }
        
        public override Table ConvertToTable(Dictionary<string, object> value)
        {
            value.TryGetValue("ID_Должности", out var idObject);
            value.TryGetValue("Наименованиеи", out var наименованиеObject);
            value.TryGetValue("Ставка", out var ставкаObject);
            int id = int.Parse(idObject.ToString());
            string наименование = наименованиеObject.ToString();
            int ставка = int.Parse(ставкаObject.ToString());
            return new Должности(id, наименование, ставка);
        }

        public override Table[] ConvertToTables(List<Dictionary<string, object>> value)
        {
            var result = new Должности[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                var dictionary = value[i];
                result[i] = (Должности)ConvertToTable(dictionary);
            }

            return result;
        }
    }
}