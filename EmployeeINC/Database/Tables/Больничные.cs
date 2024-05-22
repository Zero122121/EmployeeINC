using System.Collections.Generic;
using System.Linq;

namespace EmployeeINC.Database.Tables
{
    public class Больничные : Table
    {
        public int ID_Больничные;
        public int ID_Сотрудника;
        public string Дата_завершения;
        public int Номер_больничного;
        public string Диагноз;
        public string Дата_начала;

        public Сотрудники Сотрудник => (Сотрудники)new Сотрудники().ConvertToTable(DB.Database
            .ExecuteQuery($"SELECT * FROM Сотрудники WHERE ID_Сотрудника = {ID_Сотрудника}").FirstOrDefault());

        public Больничные(int id, int idСотрудника, string датаЗавершения, int номер, string диагноз, string датаНачала)
        {
            ID_Больничные = id;
            ID_Сотрудника = idСотрудника;
            Дата_завершения = датаЗавершения;
            Номер_больничного = номер;
            Диагноз = диагноз;
            Дата_начала = датаНачала;
        }

        public Больничные()
        {
        }
        
        public override Table ConvertToTable(Dictionary<string, object> value)
        {
            value.TryGetValue("ID_Больничные", out var ID_БольничныеObject);
            value.TryGetValue("ID_Сотрудника", out var ID_СотрудникаObject);
            value.TryGetValue("Дата_завершения", out var Дата_завершенияObject);
            value.TryGetValue("Номер_больничного", out var Номер_больничногоObject);
            value.TryGetValue("Диагноз", out var ДиагнозObject);
            value.TryGetValue("Дата_начала", out var Дата_началаObject);
            int idБольничные = int.Parse(ID_БольничныеObject.ToString());
            int idСотрудника = int.Parse(ID_СотрудникаObject.ToString());
            string датаЗавершения = Дата_завершенияObject.ToString();
            int номерБольничного = int.Parse(Номер_больничногоObject.ToString());
            string диагноз = ДиагнозObject.ToString();
            string датаНачала = Дата_началаObject.ToString();
            return new Больничные(idБольничные, idСотрудника, датаЗавершения, номерБольничного, диагноз, датаНачала);
        }

        public override Table[] ConvertToTables(List<Dictionary<string, object>> value)
        {
            var result = new Больничные[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                var dictionary = value[i];
                result[i] = (Больничные)ConvertToTable(dictionary);
            }

            return result;
        }
    }
}