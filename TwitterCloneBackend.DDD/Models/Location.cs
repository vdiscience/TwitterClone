namespace TwitterCloneBackend.DDD.Models;

public class Location : Base
{

    public City City { get; set; }

    public Country Country { get; set; }
}