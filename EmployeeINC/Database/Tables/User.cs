using System.Collections.Generic;

namespace EmployeeINC.Database.Tables
{
    public class User : Table
    {
        public int ID_User;
        public string Login;
        public string Password;
        public string role;

        public User(int id, string login, string password, string role)
        {
            ID_User = id;
            Login = login;
            Password = password;
            this.role = role;
        }

        public User() { }

        public override Table ConvertToTable(Dictionary<string, object> value)
        {
            value.TryGetValue("ID_User", out var idObject);
            value.TryGetValue("Login", out var loginObject);
            value.TryGetValue("Password", out var passwordObject);
            value.TryGetValue("role", out var roleObject);
            int id = int.Parse(idObject.ToString());
            string login = loginObject.ToString();
            string password = passwordObject.ToString();
            string role = roleObject.ToString();
            return new User(id, login, password, role);
        }

        public override Table[] ConvertToTables(List<Dictionary<string, object>> value)
        {
            var result = new User[value.Count];
            for (int i = 0; i < value.Count; i++)
            {
                var dictionary = value[i];
                result[i] = (User)ConvertToTable(dictionary);
            }

            return result;
        }
    }
}