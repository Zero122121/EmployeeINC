using System.Collections.Generic;
using System.Linq;

namespace EmployeeINC.Database.Tables
{
    public class Отпуски : Table
    {
        public int ID_Отпуска;
        public string Дата_начала;
        public string Дата_завершения;
        public string Вид_отпуска;
        public int ID_Сотрудника;

        public Сотрудники Сотрудник => (Сотрудники)new Сотрудники().ConvertToTable(DB.Database
            .ExecuteQuery($"SELECT * FROM Сотрудники WHERE ID_Сотрудника = {ID_Сотрудника}").FirstOrDefault());

        public Отпуски(int id, string датаНачала, string датаЗавершения, int idСотрудника, string видОтпуска)
        {
            ID_Отпуска = id;
            Дата_начала = датаНачала;
            Дата_завершения = датаЗавершения;
            ID_Сотрудника = idСотрудника;
            Вид_отпуска = видОтпуска;
        }

        public Отпуски()
        {
            Вид_отпуска = "";
            ID_Отпуска = -1;
            Дата_начала = "";
            Дата_завершения = "";
            ID_Сотрудника = -1;
        }

        public override Table ConvertToTable(Dictionary<string, object> value)
        {
            value.TryGetValue("ID_Отпуска", out var idObject);
            value.TryGetValue("Дата_начала", out var dateStartObject);
            value.TryGetValue("Дата_завершения", out var dateEndObject);
            value.TryGetValue("ID_Сотрудника", out var idСотрудникаObject);
            value.TryGetValue("вид_отпуска", out var вид_отпускаObject);
            int id = int.Parse(idObject.ToString());
            string dateStart = dateStartObject.ToString();
            string dateEnd = dateEndObject.ToString();
            string вид_отпуска = вид_отпускаObject.ToString();
            int idСотрудника = int.Parse(idСотрудникаObject.ToString());
            return new Отпуски(id, dateStart, dateEnd, idСотрудника, вид_отпуска);
        }

        public override Table[] ConvertToTables(List<Dictionary<string, object>> value)
        {
            var result = new Отпуски[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                var dictionary = value[i];
                result[i] = (Отпуски)ConvertToTable(dictionary);
            }

            return result;
        }
    }
}