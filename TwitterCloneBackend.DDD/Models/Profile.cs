namespace TwitterCloneBackend.DDD.Models;

public class Profile : Base
{

    public string ProfileName { get; set; }

    public string Biography { get; set; }

    public Location Location { get; set; }

    public string Website { get; set; }

    public DateTime BirthDate { get; set; }

    public List<Tweet> Tweets { get; set; } = new List<Tweet>();
}