namespace TwitterCloneBackend.DDD.Models
{
    public class Base
    {
        public Base()
        {
            this.Id = Guid.NewGuid();
            this.DateTimeEntered = DateTime.Now;
        }
        public Guid Id { get; set; }

        public DateTime DateTimeEntered { get; set; }

        public byte Deleted { get; set; }

        public DateTime DateTimeDeleted { get; set; }

    }
}
