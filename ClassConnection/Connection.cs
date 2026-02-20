using ClassModule;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;

namespace ClassConnection
{
    public class Connection
    {
        public List<User> users = new List<User>();
        public List<Call> calls = new List<Call>();

        public enum tables
        {
            users, calls
        }
        public string localPath = "";

        public bool ItsNumber(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ItsOnlyFIO(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            foreach (char c in str)
            {
                if (!(char.IsLetter(c) || c == ' ' || c == '-' || c == '\''))
                {
                    return false;
                }
            }
            return true;
        }


        public OleDbDataReader QueryAccess(string query)
        {
            //try
            //{
                localPath = Directory.GetCurrentDirectory();
                OleDbConnection connect = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\student-a502.PERMAVIAT\Desktop\accessbase.accdb");
                connect.Open();
                OleDbCommand cmd = new OleDbCommand(query, connect);
                OleDbDataReader reader = cmd.ExecuteReader();
                return reader;
            //}
            //catch
            //{
            //    return null;
            //}
        }
        public int SetLastId(tables tabel)
        {
            try
            {
                LoadData(tabel);
                switch (tabel.ToString())
                {
                    case "users":
                        if (users.Count >= 1)
                        {
                            int max_status = users[0].Id;
                            max_status = users.Max(x => x.Id);
                            return max_status + 1;
                        }
                        else return 1;
                    case "calls":
                        if (calls.Count >= 1)
                        {
                            int max_status = calls[0].Id;
                            max_status = calls.Max(x => x.Id);
                            return max_status + 1;
                        }
                        else return 1;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        public void LoadData(tables zap)
        {

            try
            {

                OleDbDataReader itemQuery = QueryAccess("select * from [" + zap.ToString() + "]order by [Код]");
                if (itemQuery == null)
                {
                    Console.WriteLine($"Ошибка: QeuryAccess вернул null для таблицы {zap}");
                    return;
                }
                if (zap.ToString() == "users")
                {
                    users.Clear();
                    while (itemQuery.Read())
                    {
                        User newE1 = new User();
                        newE1.Id = Convert.ToInt32(itemQuery.GetValue(0));
                        newE1.Phone_num = Convert.ToString(itemQuery.GetValue(1));
                        newE1.Fio_user = Convert.ToString(itemQuery.GetValue(2));
                        newE1.Passport_data = Convert.ToString(itemQuery.GetValue(3));
                        users.Add(newE1);
                    }
                }
                if (zap.ToString() == "calls")
                {
                    calls.Clear();
                    while (itemQuery.Read())
                    {
                        Call NewE1 = new Call();
                        NewE1.Id = Convert.ToInt32(itemQuery.GetValue(0));
                        NewE1.User_id = Convert.ToInt32(itemQuery.GetValue(1));
                        NewE1.Category_call = Convert.ToInt32(itemQuery.GetValue(2));
                        NewE1.Date = Convert.ToString(itemQuery.GetValue(3));
                        NewE1.Time_start = Convert.ToString(itemQuery.GetValue(4));
                        NewE1.Time_end = Convert.ToString(itemQuery.GetValue(5));
                        calls.Add(NewE1);
                    }
                }
                if (itemQuery != null) itemQuery.Close();
            }
            catch
            {
                Console.WriteLine("NULL");
            }
        }
    }
}
