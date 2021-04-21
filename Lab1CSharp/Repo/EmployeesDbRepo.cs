using System;
using System.Collections.Generic;
using Lab1CSharp.Domain;
using Lab1CSharp.Domain.Validators;
using Lab1CSharp.Repo.Interfaces;
using Mono.Data.Sqlite;

namespace Lab1CSharp.Repo
{
    public class EmployeesDbRepo: IEmployeeRepo
    {
        public Employee GetEmployeeByUser(string user)
        {
            string select = "select E.id eid, name, position, office, location, username, password from Employees E inner join Offices O on O.id = E.office where username=@user";
            Employee art = null;
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("user", user);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                art = new Employee(reader.GetInt64(0),
                                    reader.GetFieldValue<string>(1),
                                    reader.GetFieldValue<string>(2),
                                    new Office(reader.GetInt64(3), reader.GetFieldValue<string>(4)),
                                    reader.GetFieldValue<string>(5),
                                    reader.GetFieldValue<string>(6)
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

        public string GetPasswordByUser(string user)
        {
            if (!UsernameExists(user)) return null;
            string select = "select E.id eid, name, position, office, location, username, password from Employees E inner join Offices O on O.id = E.office where username=@user";
            string art = null;
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("user", user);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                art = reader.GetFieldValue<string>(6);
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

        public bool UsernameExists(string user)
        {
            string select = "select E.id eid, name, position, office, location, username, password from Employees E inner join Offices O on O.id = E.office where username=@user";
            bool res = false;
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("user", user);
                        
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                res = true;
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

            return res;
        }

        private string _conn;
        private IValidator<Employee> _validator = new EmployeeValidator();
        
        public string Conn
        {
            get => _conn;
            set => _conn = value;
        }
        public EmployeesDbRepo(string conn)
        {
            _conn = conn;
        }

        public Employee FindOne(long id)
        {
            string select = "select E.id eid, name, position, office, location, username, password from Employees E inner join Offices O on O.id = E.office where eid=@id";
            Employee art = null;
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
                                art = new Employee(reader.GetInt64(0),
                                    reader.GetFieldValue<string>(1),
                                    reader.GetFieldValue<string>(2),
                                    new Office(reader.GetInt64(3), reader.GetFieldValue<string>(4)),
                                    reader.GetFieldValue<string>(5),
                                    reader.GetFieldValue<string>(6)
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

        public IDictionary<long, Employee> FindAll()
        {
            IDictionary<long, Employee> lst = new Dictionary<long, Employee>();
            string select = "select E.id eid, name, position, office, location, username, password from Employees E inner join Offices O on O.id = E.office";
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
                                    lst.Add(reader.GetInt64(0), new Employee(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1),
                                        reader.GetFieldValue<string>(2),
                                        new Office(reader.GetInt64(3), reader.GetFieldValue<string>(4)),
                                        reader.GetFieldValue<string>(5),
                                        reader.GetFieldValue<string>(6)));
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

        public IList<Employee> GetAllList()
        {
            IList<Employee> lst = new List<Employee>();
            string select = "select E.id eid, name, position, office, location, username, password from Employees E inner join Offices O on O.id = E.office";
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
                                    lst.Add(new Employee(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1),
                                        reader.GetFieldValue<string>(2),
                                        new Office(reader.GetInt64(3), reader.GetFieldValue<string>(4)),
                                        reader.GetFieldValue<string>(5),
                                        reader.GetFieldValue<string>(6)));
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

        public Employee Save(Employee entity)
        {
            // if (entity == null) throw new ArgumentException("Can't be null");
            _validator.Validate(entity);
            
            Employee art = null;
            art = this.FindOne(entity.Id);
            if (art != null) return art;
            string select = "insert into Employees (id, name, position, office, username, password) VALUES (@id, @name, @position, @office, @user, @pass)";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("id", entity.Id);
                        cmd.Parameters.AddWithValue("name", entity.Name);
                        cmd.Parameters.AddWithValue("position", entity.Position);
                        cmd.Parameters.AddWithValue("office", entity.Office.Id);
                        cmd.Parameters.AddWithValue("user", entity.Username);
                        cmd.Parameters.AddWithValue("pass", entity.Password);

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

        public Employee Delete(long id)
        {
            Employee art = null;
            art = this.FindOne(id);
            if (art == null) return null;
            string select = "delete from Employees where id=@id";
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

        public Employee Update(Employee entity)
        {
            // if (entity == null) throw new ArgumentException("Can't be null");
            _validator.Validate(entity);
            
            Employee art = null;
            art = this.FindOne(entity.Id);
            if (art == null) return entity;
            art = null;
            string select = "update Employees set name=@name, position=@position, office=@office, username=@user, password=@pass where id=@id";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("name", entity.Name);
                        cmd.Parameters.AddWithValue("position", entity.Position);
                        cmd.Parameters.AddWithValue("office", entity.Office.Id);
                        cmd.Parameters.AddWithValue("id", entity.Id);
                        cmd.Parameters.AddWithValue("user", entity.Username);
                        cmd.Parameters.AddWithValue("pass", entity.Password);

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

        public IList<Employee> FilterByName(string name)
        {
            IList<Employee> lst = new List<Employee>();
            string select = "select E.id eid, name, position, office, location, username, password from Employees E inner join Offices O on O.id = E.office where name=@name";
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
                                    lst.Add(new Employee(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1),
                                        reader.GetFieldValue<string>(2),
                                        new Office(reader.GetInt64(3), reader.GetFieldValue<string>(4)),
                                        reader.GetFieldValue<string>(5),
                                        reader.GetFieldValue<string>(6)));
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

        public IList<Employee> FilterByPosition(string position)
        {
            IList<Employee> lst = new List<Employee>();
            string select = "select E.id eid, name, position, office, location, username, password from Employees E inner join Offices O on O.id = E.office where position=@pos";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("pos", position);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    lst.Add(new Employee(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1),
                                        reader.GetFieldValue<string>(2),
                                        new Office(reader.GetInt64(3), reader.GetFieldValue<string>(4)),
                                        reader.GetFieldValue<string>(5),
                                        reader.GetFieldValue<string>(6)));
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

        public IList<Employee> FilterByOffice(long office)
        {
            IList<Employee> lst = new List<Employee>();
            string select = "select E.id eid, name, position, office, location, username, password from Employees E inner join Offices O on O.id = E.office where office=@office";
            try
            {
                using (var conn = new SqliteConnection(_conn))
                {
                    conn.Open();
                    using (SqliteCommand cmd = new SqliteCommand(select, conn))
                    {
                        cmd.Parameters.AddWithValue("office", office);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                { 
                                    lst.Add(new Employee(reader.GetInt64(0),
                                        reader.GetFieldValue<string>(1),
                                        reader.GetFieldValue<string>(2),
                                        new Office(reader.GetInt64(3), reader.GetFieldValue<string>(4)),
                                        reader.GetFieldValue<string>(5),
                                        reader.GetFieldValue<string>(6)));
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