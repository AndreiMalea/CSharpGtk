using System;
using System.Collections.Generic;
using Lab1CSharp.Domain;
using Lab1CSharp.Domain.Validators;
using Lab1CSharp.Repo.Interfaces;
using Mono.Data.Sqlite;

namespace Lab1CSharp.Repo
{
    public class ShowDbRepo: IShowRepo
    {
        private string _conn;
        private IValidator<Show> _validator = new ShowValidator();
        
        public string Conn
        {
            get => _conn;
            set => _conn = value;
        }
        public ShowDbRepo(string conn)
        {
            _conn = conn;
        }
        
        public Show FindOne(long id)
        {
            string select = "select S.id, A.id aid, A.name, A.genre, S.ticketNumber, S.date from Shows S inner join Artists A on A.id = S.artist where S.id=@id";
            Show art = null;
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
                                
                                art = new Show(reader.GetInt64(0),
                                    new Artist(reader.GetInt64(1),
                                        reader.GetFieldValue<string>(2),
                                        reader.GetFieldValue<string>(3)),
                                    reader.GetInt32(4),
                                    DateTime.Parse(reader.GetFieldValue<string>(5)).Date
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

        public IDictionary<long, Show> FindAll()
        {
            IDictionary<long, Show> lst = new Dictionary<long, Show>();
            string select = "select S.id, A.id aid, A.name, A.genre, S.ticketNumber, S.date from Shows S inner join Artists A on A.id = S.artist";
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
                                    lst.Add(reader.GetInt64(0), new Show(reader.GetInt64(0),
                                        new Artist(reader.GetInt64(1),
                                            reader.GetFieldValue<string>(2),
                                            reader.GetFieldValue<string>(3)),
                                        reader.GetInt32(4),
                                        DateTime.Parse(reader.GetFieldValue<string>(5)).Date
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

        public IList<Show> GetAllList()
        {
            IList<Show> lst = new List<Show>();
            string select = "select S.id, A.id aid, A.name, A.genre, S.ticketNumber, S.date from Shows S inner join Artists A on A.id = S.artist";
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
                                    lst.Add(new Show(reader.GetInt64(0),
                                        new Artist(reader.GetInt64(1),
                                            reader.GetFieldValue<string>(2),
                                            reader.GetFieldValue<string>(3)),
                                        reader.GetInt32(4),
                                        DateTime.Parse(reader.GetFieldValue<string>(5)).Date
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

        public Show Save(Show entity)
        {
            _validator.Validate(entity);
            
            Show art = null;
            art = this.FindOne(entity.Id);
            if (art != null) return art;
            string select = "insert into Shows (id, artist, ticketNumber, date) VALUES (@id, @artist, @number, @date )";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", entity.Id);
                        cmd.Parameters.AddWithValue("artist", entity.Artist.Id);
                        cmd.Parameters.AddWithValue("number", entity.TicketNumber);
                        
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

        public Show Delete(long id)
        {
            Show art = null;
            art = this.FindOne(id);
            if (art == null) return null;
            string select = "delete from Shows where id=@id";
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

        public Show Update(Show entity)
        {
            // if (entity == null) throw new ArgumentException("Can't be null");
            _validator.Validate(entity);
            
            Show art = null;
            art = this.FindOne(entity.Id);
            if (art == null) return entity;
            art = null;
            string select = "update Shows set artist=@artist, ticketNumber=@ticketNumber, date=@date where id=@id";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
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
                        
                        cmd.Parameters.AddWithValue("artist", entity.Artist.Id);
                        cmd.Parameters.AddWithValue("ticketNumber", entity.TicketNumber);
                        cmd.Parameters.AddWithValue("date", entity.Date.Year+"-"+month+"-"+day);
                        cmd.Parameters.AddWithValue("id", entity.Id);

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

        public IList<Show> FilterByDate(DateTime dateTime)
        {
            IList<Show> lst = new List<Show>();
            string select = "select S.id, A.id aid, A.name, A.genre, S.ticketNumber, S.date from Shows S inner join Artists A on A.id = S.artist where S.date=@date";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        string month;
                        if (dateTime.Month <= 9)
                        {
                            month = "0" + dateTime.Month;
                        }
                        else
                        {
                            month = dateTime.Month.ToString();
                        }
                        
                        string day;
                        if (dateTime.Day <= 9)
                        {
                            day = "0" + dateTime.Day;
                        }
                        else
                        {
                            day = dateTime.Day.ToString();
                        }
                        
                        cmd.Parameters.AddWithValue("date", dateTime.Year+"-"+month+"-"+day);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    lst.Add(new Show(reader.GetInt64(0),
                                        new Artist(reader.GetInt64(1),
                                            reader.GetFieldValue<string>(2),
                                            reader.GetFieldValue<string>(3)),
                                        reader.GetInt32(4),
                                        DateTime.Parse(reader.GetFieldValue<string>(5)).Date
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
            
            throw new NotImplementedException();
        }

        public IList<Show> FilterByArtist(long artist)
        {
            IList<Show> lst = new List<Show>();
            string select = "select S.id, A.id aid, A.name, A.genre, S.ticketNumber, S.date from Shows S inner join Artists A on A.id = S.artist where artist=@artist";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("artist", artist);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    lst.Add(new Show(reader.GetInt64(0),
                                        new Artist(reader.GetInt64(1),
                                            reader.GetFieldValue<string>(2),
                                            reader.GetFieldValue<string>(3)),
                                        reader.GetInt32(4),
                                        DateTime.Parse(reader.GetFieldValue<string>(5)).Date
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
    }
}