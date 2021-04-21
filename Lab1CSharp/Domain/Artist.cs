using System;

namespace Lab1CSharp.Domain
{
    public class Artist: Entity<long>
    {
        private String _name;
        private String _genre;

        public string Name
        {
            get => _name;
            set => _name = value;
        }

        public string Genre
        {
            get => _genre;
            set => _genre = value;
        }

        public Artist(long id, string name, string genre)
        {
            this.Id = id;
            _name = name;
            _genre = genre;
        }

        public override string ToString()
        {
            return "{Artist : id="+this.Id+", name="+this.Name+", genre="+this.Genre+"}";
        }
    }
}