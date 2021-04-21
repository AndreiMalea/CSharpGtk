using System;

namespace Lab1CSharp.Domain
{
    public class Employee: Entity<long>
    {
        private String _name;
        private String _position;
        private Office _office;
        private String _username;
        private String _password;

        public string Username
        {
            get => _username;
            set => _username = value;
        }

        public string Password
        {
            get => _password;
            set => _password = value;
        }

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Position
        {
            get => _position;
            set => _position = value;
        }

        public Office Office
        {
            get => _office;
            set => _office = value;
        }

        public Employee(long id, string name, string position, Office office, string username, string password)
        {
            this.Id = id;
            _name = name;
            _position = position;
            _office = office;
            this._username = username;
            this._password = password;
        }

        public override string ToString()
        {
            return "{Employee : id="+ this.Id +
                   ", name="+ this.Name +
                   ", position="+ this.Position +
                   ", office="+ this.Office.ToString()+
                   ", username="+ this.Username+
                   ", password="+ this.Password+
                   "}";
        }
    }
}