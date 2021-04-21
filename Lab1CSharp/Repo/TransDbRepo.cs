using System;
using System.Collections.Generic;
using Lab1CSharp.Domain;
using Lab1CSharp.Domain.Validators;
using Lab1CSharp.Repo.Interfaces;
using Mono.Data.Sqlite;

namespace Lab1CSharp.Repo
{
    public class TransDbRepo : ITransactionRepo
    {
        private string _conn;
        private IValidator<Transaction> _validator = new TransValidator();

        public TransDbRepo(string conn)
        {
            _conn = conn;
        }

        public Transaction FindOne(int id)
        {
            // string select = "select * from Transactions inner join Shows S on S.id = Transactions.show inner join Artists A on A.id = S.artist where id=(?)";
            string select =
                "select T.id tid, T.client, A.id aid, A.name, A.genre, S.id sid, S.ticketNumber, S.date sdate, T.date tdate, T.ticketNumber from Transactions T inner join Shows S on S.id = T.show inner join Artists A on A.id = S.artist where tid=@id";
            Transaction art = null;
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                art = new Transaction(reader.GetInt32(0),
                                    reader.GetFieldValue<string>(1),
                                    new Show(reader.GetInt64(5),
                                        new Artist(reader.GetInt64(2),
                                            reader.GetFieldValue<string>(3),
                                            reader.GetFieldValue<string>(4)),
                                        reader.GetInt32(6),
                                        DateTime.Parse(reader.GetFieldValue<string>(7))),
                                    DateTime.Parse(reader.GetFieldValue<string>(8)), reader.GetInt32(9)
                                );
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

            return art;
        }

        public IDictionary<int, Transaction> FindAll()
        {
            IDictionary<int, Transaction> lst = new Dictionary<int, Transaction>();
            string select =
                "select T.id tid, T.client, A.id aid, A.name, A.genre, S.id sid, S.ticketNumber, S.date sdate, T.date tdate, T.ticketNumber from Transactions T inner join Shows S on S.id = T.show inner join Artists A on A.id = S.artist";

            try
            {
                using (var conn = new SqliteConnection(_conn))
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
                                    lst.Add(reader.GetInt32(0),new Transaction(reader.GetInt32(0),
                                        reader.GetFieldValue<string>(1),
                                        new Show(reader.GetInt64(5),
                                            new Artist(reader.GetInt64(2),
                                                reader.GetFieldValue<string>(3),
                                                reader.GetFieldValue<string>(4)),
                                            reader.GetInt32(6),
                                            DateTime.Parse(reader.GetFieldValue<string>(7))),
                                        DateTime.Parse(reader.GetFieldValue<string>(8)), reader.GetInt32(9)
                                    ));
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

            return lst;
        }

        public IList<Transaction> GetAllList()
        {
            IList<Transaction> lst = new List<Transaction>();
            string select =
                "select T.id tid, T.client, A.id aid, A.name, A.genre, S.id sid, S.ticketNumber, S.date sdate, T.date tdate, T.ticketNumber from Transactions T inner join Shows S on S.id = T.show inner join Artists A on A.id = S.artist";

            try
            {
                using (var conn = new SqliteConnection(_conn))
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
                                    lst.Add(new Transaction(reader.GetInt32(0),
                                        reader.GetFieldValue<string>(1),
                                        new Show(reader.GetInt64(5),
                                            new Artist(reader.GetInt64(2),
                                                reader.GetFieldValue<string>(3),
                                                reader.GetFieldValue<string>(4)),
                                            reader.GetInt32(6),
                                            DateTime.Parse(reader.GetFieldValue<string>(7))),
                                        DateTime.Parse(reader.GetFieldValue<string>(8)), reader.GetInt32(9)
                                    ));
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

            return lst;
        }

        public Transaction Save(Transaction entity)
        {
            _validator.Validate(entity);
            
            Transaction art = null;
            art = this.FindOne(entity.Id);
            if (art != null) return art;
            string select = "insert into Transactions (id, client, show, date, ticketNumber) VALUES (@id, @client, @show, @date, @ticket)";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", entity.Id);
                        cmd.Parameters.AddWithValue("client", entity.Client);
                        cmd.Parameters.AddWithValue("show", entity.Show.Id);

                        string month;
                        if (entity.Date.Month <= 9)
                        {
                            month = "0" + entity.Date.Month;
                        }
                        else
                        {
                            month = entity.Date.Month.ToString();
                        }
                        
                        string day;
                        if (entity.Date.Day <= 9)
                        {
                            day = "0" + entity.Date.Day;
                        }
                        else
                        {
                            day = entity.Date.Day.ToString();
                        }
                        
                        cmd.Parameters.AddWithValue("date", entity.Date.Year+"-"+month+"-"+day);
                        cmd.Parameters.AddWithValue("ticket", entity.TicketNumber);

                        var executed = cmd.ExecuteNonQuery();
                        if (executed == 0) art = entity;
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return art;
        }

        public Transaction Delete(int id)
        {
            Transaction art = null;
            art = this.FindOne(id);
            if (art == null) return null;
            string select = "delete from Transactions where id=@id";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", id);
                        
                        var executed = cmd.ExecuteNonQuery();
                        if (executed == 0) art=null;
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return art;
        }

        public Transaction Update(Transaction entity)
        {
            _validator.Validate(entity);
            
            Transaction art = null;
            art = this.FindOne(entity.Id);
            if (art == null) return entity;
            art = null;
            // string select = "update Employees set name=@name, position=@position, office=@office, username=@user, password=@pass where id=@id";
            string select = "update Transactions set client=@client, show=@show, date=@date, ticketNumber=@ticket where id=@id";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", entity.Id);
                        cmd.Parameters.AddWithValue("client", entity.Client);
                        cmd.Parameters.AddWithValue("show", entity.Show.Id);

                        string month;
                        if (entity.Date.Month <= 9)
                        {
                            month = "0" + entity.Date.Month;
                        }
                        else
                        {
                            month = entity.Date.Month.ToString();
                        }
                        
                        string day;
                        if (entity.Date.Day <= 9)
                        {
                            day = "0" + entity.Date.Day;
                        }
                        else
                        {
                            day = entity.Date.Day.ToString();
                        }
                        
                        cmd.Parameters.AddWithValue("date", entity.Date.Year+"-"+month+"-"+day);
                        cmd.Parameters.AddWithValue("ticket", entity.TicketNumber);

                        var executed = cmd.ExecuteNonQuery();
                        if (executed == 0) art = entity;
                    }
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return art;
        }
    }
}