using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Mono.Data.Sqlite;

namespace Lab1CSharp.Domain
{
    public class Transaction: Entity<int>
    {
        private static ISet<int> _idList = new HashSet<int>();
        private string _client;
        private Show _show;
        private DateTime _date;
        private int _ticketNumber;

        public static ISet<int> IdList
        {
            get => _idList;
            set => _idList = value;
        }

        public static void IdListInit()
        {
            String rv = null;
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            try
            {
                configMap.ExeConfigFilename =
                    Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + "/bd.config";
                // configMap.ExeConfigFilename = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "/bd.config";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            var set = ConfigurationManager.OpenMappedExeConfiguration(configMap,
                ConfigurationUserLevel.None).ConnectionStrings.ConnectionStrings["SQLite"];
            if (set != null)
            {
                rv = set.ConnectionString;
            }
            
            string select = "select id from Transactions";
            try
            {
                using (var conn = new SqliteConnection(rv))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    _idList.Add(reader.GetInt32(0));
                                }
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
        
        public string Client
        {
            get => _client;
            set => _client = value;
        }

        public Show Show
        {
            get => _show;
            set => _show = value;
        }

        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        public int TicketNumber
        {
            get => _ticketNumber;
            set => _ticketNumber = value;
        }

        public Transaction(string client, Show show, DateTime date, int ticketNumber)
        {
            Random random = new Random();
            int id = random.Next();

            while (_idList.Contains(id))
            {
                id = random.Next();
            }

            this.Id = id;
            _client = client;
            _show = show;
            _date = date;
            _ticketNumber = ticketNumber;
        }
        
        public Transaction(int id, string client, Show show, DateTime date, int ticketNumber)
        {
            this.Id = id;
            _client = client;
            _show = show;
            _date = date;
            _ticketNumber = ticketNumber;
        }

        public override string ToString()
        {
            return "{Transaction: id=" +this.Id+
                   ", client="+this._client +
                   ", show="+this._show +
                   ", date="+this._date +
                   ", ticketNumber="+this._ticketNumber +
                   "}";
        }
    }
}