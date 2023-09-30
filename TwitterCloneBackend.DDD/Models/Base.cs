namespace TwitterCloneBackend.Entities.Models
{
    public class Base
    {
        public Base()
        {
            Id = Guid.NewGuid();
            DateTimeEntered = DateTime.Now;
        }
        public Guid Id { get; set; }

        public DateTime DateTimeEntered { get; set; }

        public byte Deleted { get; set; }

        public DateTime DateTimeDeleted { get; set; }

    }
}
