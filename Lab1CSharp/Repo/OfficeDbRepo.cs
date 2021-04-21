using System;
using System.Collections.Generic;
using Lab1CSharp.Domain;
using Lab1CSharp.Domain.Validators;
using Lab1CSharp.Repo.Interfaces;
using Mono.Data.Sqlite;

namespace Lab1CSharp.Repo
{
    public class OfficeDbRepo: IOfficeRepo
    {
        private string _conn;
        private IValidator<Office> _validator = new OfficeValidator();
        
        public string Conn
        {
            get => _conn;
            set => _conn = value;
        }
        
        public OfficeDbRepo(string conn)
        {
            _conn = conn;
        }
        
        public Office FindOne(long id)
        {
            // if (id == null) throw new ArgumentException("Can't be null");
            string select = "select * from Offices where id=@id";
            Office art = null;
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
                                art = new Office(reader.GetInt64(0),
                                    reader.GetFieldValue<string>(1));
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

        public IDictionary<long, Office> FindAll()
        {
            IDictionary<long, Office> lst = new Dictionary<long, Office>();
            string select = "select * from Offices";
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
                                    lst.Add(reader.GetInt64(0), new Office(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1)));
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

        public IList<Office> GetAllList()
        {
            IList<Office> lst = new List<Office>();
            string select = "select * from Offices";
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
                                    lst.Add(new Office(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1)));
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

        public Office Save(Office entity)
        {
            // if (entity == null) throw new ArgumentException("Can't be null");
            _validator.Validate(entity);
            
            Office art = null;
            art = this.FindOne(entity.Id);
            if (art != null) return art;
            string select = "insert into Offices (id, location) VALUES (@id, @location)";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", entity.Id);
                        cmd.Parameters.AddWithValue("location", entity.Location);
                        
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

        public Office Delete(long id)
        {
            Office art = null;
            art = this.FindOne(id);
            if (art == null) return null;
            string select = "delete from Offices where id=@id";
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

        public Office Update(Office entity)
        {
            // if (entity == null) throw new ArgumentException("Can't be null");
            _validator.Validate(entity);
            
            Office art = null;
            art = this.FindOne(entity.Id);
            if (art == null) return entity;
            art = null;
            string select = "update Offices set location=@location where id=@id";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", entity.Id);
                        cmd.Parameters.AddWithValue("location", entity.Location);
                        
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

        public IList<Office> FilterByLocation(string location)
        {
            IList<Office> lst = new List<Office>();
            string select = "select * from Offices where location=@location";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("location", location);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    lst.Add(new Office(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1)));
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