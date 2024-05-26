﻿using System.Collections.Generic;
using System.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Media.Imaging;
using DB.Utils;

namespace EmployeeINC.Database.Tables
{
    public class Сотрудники : Table
    {
        public int ID_Сотрудника;
        public string Фамилия;
        public string Имя;
        public string Отчество;
        public string Телефон;
        public string tg_username;
        public int ID_Отдел;
        public int ID_Должность;
        public string Дата_начала_работы;
        public byte[] picture;
        public BitmapImage pictureBitmap;
        
        public Отделы Отдел => (Отделы)new Отделы().ConvertToTable(DB.Database
            .ExecuteQuery($"SELECT * FROM Отделы WHERE ID_Отдела = {ID_Отдел}").FirstOrDefault());

        public Должности Должность => (Должности)new Должности().ConvertToTable(DB.Database
            .ExecuteQuery($"SELECT * FROM Должности WHERE ID_Должности = {ID_Должность}").FirstOrDefault());

        public Сотрудники(int id, string фамилия, string имя, string отчество, string телефон, int idОтдел,
            int idДолжность, string датаНачалаРаботы, string tgUsername, byte[] pictureBytes)
        {
            ID_Сотрудника = id;
            Фамилия = фамилия;
            Имя = имя;
            Отчество = отчество;
            Телефон = телефон;
            tg_username = tgUsername;
            ID_Отдел = idОтдел;
            ID_Должность = idДолжность;
            Дата_начала_работы = датаНачалаРаботы;
            picture = pictureBytes;
            pictureBitmap = pictureBytes.ConvertByteArrayToBitmapImage();
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
            value.TryGetValue("tg_username", out var tgUsernameObject);
            value.TryGetValue("picture", out object pictureObject);
            int id = int.Parse(ID_СотрудникаObject.ToString());
            string фамилия = фамилияObject.ToString();
            string имя = имяObject.ToString();
            string отчество = отчествоObject.ToString();
            string телефон = телефонObject.ToString();
            string датаНачалаРаботы = датаНачалаРаботыObject.ToString();
            string tgUsername = tgUsernameObject.ToString();
            int idОтдел = int.Parse(отделObject.ToString());
            int idДолжность = int.Parse(должностьObject.ToString());
            byte[] pictureBytes = null;
            if (pictureObject is byte[] bytes) pictureBytes = bytes;
            
            return new Сотрудники(id, фамилия, имя, отчество, телефон, idОтдел, idДолжность, датаНачалаРаботы, tgUsername,
                pictureObject == null ? new byte[] {} : pictureBytes);
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