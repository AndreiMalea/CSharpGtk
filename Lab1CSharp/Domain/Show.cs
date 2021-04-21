using System;

namespace Lab1CSharp.Domain
{
    public class Show: Entity<long>
    {
        private Artist _artist;
        private int _ticketNumber;
        private DateTime _date;

        public DateTime Date
        {
            get => _date;
            set => _date = value;
        }

        public Artist Artist
        {
            get => _artist;
            set => _artist = value;
        }

        public int TicketNumber
        {
            get => _ticketNumber;
            set => _ticketNumber = value;
        }

        public Show(long id, Artist artist, int ticketNumber, DateTime date)
        {
            this.Id = id;
            _artist = artist;
            _ticketNumber = ticketNumber;
            _date = date;
        }

        public void DecreaseTicketNumber(int no)
        {
            _ticketNumber -= no;
        }
        
        public override string ToString()
        {
            return "{Show: id="+this.Id+
                   ", artist="+this.Artist+
                   ", ticketNumber="+this.TicketNumber+
                   ", date="+this._date+
                   "}";
        }
    }
}