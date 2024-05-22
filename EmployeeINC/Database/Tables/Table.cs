using System.Collections.Generic;

namespace EmployeeINC.Database.Tables
{
    public abstract class Table
    {
        public abstract Table ConvertToTable(Dictionary<string, object> value);
        public abstract Table[] ConvertToTables(List<Dictionary<string, object>> value);
    }
}