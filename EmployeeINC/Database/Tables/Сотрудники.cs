using System.Collections.Generic;
using System.Linq;

namespace EmployeeINC.Database.Tables
{
    public class Сотрудники : Table
    {
        public int ID_Сотрудника;
        public string Фамилия;
        public string Имя;
        public string Отчество;
        public string Телефон;
        public int ID_Отдел;
        public int ID_Должность;
        public string Дата_начала_работы;
        
        public Отделы Отдел => (Отделы)new Отделы().ConvertToTable(DB.Database
            .ExecuteQuery($"SELECT * FROM Отделы WHERE ID_Отдела = {ID_Отдел}").FirstOrDefault());

        public Должности Должность => (Должности)new Должности().ConvertToTable(DB.Database
            .ExecuteQuery($"SELECT * FROM Должности WHERE ID_Должности = {ID_Должность}").FirstOrDefault());

        public Сотрудники(int id, string фамилия, string имя, string отчество, string телефон, int idОтдел,
            int idДолжность, string датаНачалаРаботы)
        {
            ID_Сотрудника = id;
            Фамилия = фамилия;
            Имя = имя;
            Отчество = отчество;
            Телефон = телефон;
            ID_Отдел = idОтдел;
            ID_Должность = idДолжность;
            Дата_начала_работы = датаНачалаРаботы;
        }

        public Сотрудники() { }

        public override Table ConvertToTable(Dictionary<string, object> value)
        {
            value.TryGetValue("ID_Сотрудника", out var ID_СотрудникаObject);
            value.TryGetValue("Фамилия", out var фамилияObject);
            value.TryGetValue("Имя", out var имяObject);
            value.TryGetValue("Отчество", out var отчествоObject);
            value.TryGetValue("Телефон", out var телефонObject);
            value.TryGetValue("Отдел", out var отделObject);
            value.TryGetValue("Должность", out var должностьObject);
            value.TryGetValue("Дата_начала_работы", out var датаНачалаРаботыObject);
            int id = int.Parse(ID_СотрудникаObject.ToString());
            string фамилия = фамилияObject.ToString();
            string имя = имяObject.ToString();
            string отчество = отчествоObject.ToString();
            string телефон = телефонObject.ToString();
            string датаНачалаРаботы = датаНачалаРаботыObject.ToString();
            int idОтдел = int.Parse(отделObject.ToString());
            int idДолжность = int.Parse(должностьObject.ToString());
            return new Сотрудники(id, фамилия, имя, отчество, телефон, idОтдел, idДолжность, датаНачалаРаботы);
        }

        public override Table[] ConvertToTables(List<Dictionary<string, object>> value)
        {
            var result = new Сотрудники[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                var dictionary = value[i];
                result[i] = (Сотрудники)ConvertToTable(dictionary);
            }

            return result;
        }
    }
}