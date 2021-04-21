using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Lab1CSharp.Domain;
using Lab1CSharp.Domain.Validators;
using Lab1CSharp.Repo.Interfaces;
using Mono.Data.Sqlite;

namespace Lab1CSharp.Repo
{
    public class ArtistDbRepo: IArtistRepo
    {
        private string _conn;
        private IValidator<Artist> _validator = new ArtistValidator(); 
        public string Conn
        {
            get => _conn;
            set => _conn = value;
        }

        public ArtistDbRepo(string conn)
        {
            _conn = conn;
        }

        public Artist FindOne(long id)
        {
            // if (id == null) throw new ArgumentException("Can't be null");
            string select = "select * from Artists where id=@id";
            Artist art = null;
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
                                art = new Artist(reader.GetInt64(0),
                                    reader.GetFieldValue<string>(1),
                                    reader.GetFieldValue<string>(2));
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

        public IDictionary<long, Artist> FindAll()
        {
            IDictionary<long, Artist> lst = new Dictionary<long, Artist>();
            string select = "select * from Artists";
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
                                    lst.Add(reader.GetInt64(0), new Artist(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1),
                                        reader.GetFieldValue<string>(2)));
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

        public IList<Artist> GetAllList()
        {
            IList<Artist> lst = new List<Artist>();
            string select = "select * from Artists";
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
                                    lst.Add(new Artist(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1),
                                        reader.GetFieldValue<string>(2)));
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

        public Artist Save(Artist entity)
        {
            // if (entity == null) throw new ArgumentException("Can't be null");
            _validator.Validate(entity);
            
            Artist art = null;
            art = this.FindOne(entity.Id);
            if (art != null) return art;
            string select = "insert into Artists (id, name, genre) VALUES (@id, @name, @genre)";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", entity.Id);
                        cmd.Parameters.AddWithValue("name", entity.Name);
                        cmd.Parameters.AddWithValue("genre", entity.Genre);
                        
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

        public Artist Delete(long id)
        {
            // if (id) throw new ArgumentException("Can't be null");
            Artist art = null;
            art = this.FindOne(id);
            if (art == null) return null;
            string select = "delete from Artists where id=@id";
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

        public Artist Update(Artist entity)
        {
            // if (entity == null) throw new ArgumentException("Can't be null");
            _validator.Validate(entity);
            
            Artist art = null;
            art = this.FindOne(entity.Id);
            if (art == null) return entity;
            art = null;
            string select = "update Artists set name=@name, genre=@genre where id=@id";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", entity.Id);
                        cmd.Parameters.AddWithValue("name", entity.Name);
                        cmd.Parameters.AddWithValue("genre", entity.Genre);
                        
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

        public IList<Artist> FilterByName(string name)
        {
            IList<Artist> lst = new List<Artist>();
            string select = "select * from Artists where name=@name";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("name", name);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    lst.Add(new Artist(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1),
                                        reader.GetFieldValue<string>(2)));
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
            // throw new System.NotImplementedException();
        }

        public IList<Artist> FilterByGenre(string genre)
        {
            IList<Artist> lst = new List<Artist>();
            string select = "select * from Artists where genre=@genre";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("genre", genre);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    lst.Add(new Artist(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1),
                                        reader.GetFieldValue<string>(2)));
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