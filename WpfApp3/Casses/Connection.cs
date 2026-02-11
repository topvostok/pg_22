
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.Casses
{
    public class Connection
    {
        public enum Tables
        {
            Users,
            Calls
        }

        public void LoadData(Tables table)
        {
            // Реализация загрузки данных
            switch (table)
            {
                case Tables.Users:
                    // загрузка пользователей
                    break;
                case Tables.Calls:
                    // загрузка звонков
                    break;
            }
        }
    }
}