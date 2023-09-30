namespace TwitterCloneBackend.Entities.Models
{
    /// <summary>
    /// Enterprise Domain Driven Design pattern.
    /// </summary>
    public class User : Base
    {

        public string UserName { get; set; }

        public string Password { get; set; }

        public Profile? Profile { get; set; }
    }
}
