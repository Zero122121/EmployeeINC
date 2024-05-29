using System.Collections.Generic;
using System.Linq;

namespace EmployeeINC.Database.Tables
{
    public class Document : Table
    {
        public int id_document;
        public string name;
        public byte[] document;
        public int employee_id;
        
        public Сотрудники Сотрудник => (Сотрудники)new Сотрудники().ConvertToTable(DB.Database
            .ExecuteQuery($"SELECT * FROM Сотрудники WHERE ID_Сотрудника = {employee_id}").FirstOrDefault());

        public Document(string name, int idDocument, byte[] document, int employeeId)
        {
            this.name = name;
            id_document = idDocument;
            this.document = document;
            employee_id = employeeId;
        }

        public Document() { }

        public override Table ConvertToTable(Dictionary<string, object> value)
        {
            value.TryGetValue("id_document", out var id_documentObject);
            value.TryGetValue("name", out var nameObject);
            value.TryGetValue("document", out var documentObject);
            value.TryGetValue("employee_id", out var employee_idObject);
            
            byte[] pictureBytes = null;
            if (documentObject is byte[] bytes) pictureBytes = bytes;

            return new Document(nameObject.ToString(), int.Parse(id_documentObject.ToString()),
                pictureBytes ?? new byte[] { }, int.Parse(employee_idObject.ToString()));
        }

        public override Table[] ConvertToTables(List<Dictionary<string, object>> value)
        {
            var result = new Document[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                var dictionary = value[i];
                result[i] = (Document)ConvertToTable(dictionary);
            }

            return result;
        }
    }
}