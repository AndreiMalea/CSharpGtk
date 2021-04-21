namespace Lab1CSharp.Domain
{
    public class Entity<ID>
    {
        private ID _id;

        public ID Id
        {
            get => _id;
            set => _id = value;
        }
    }
}