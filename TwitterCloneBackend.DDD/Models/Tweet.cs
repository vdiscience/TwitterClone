using TwitterCloneBackend.Entities.enums;

namespace TwitterCloneBackend.Entities.Models;

public class Tweet : Base
{

    public Profile? Profile { get; set; }


    // In case the user wants to submit media.
    public string? MediaPath { get; set; }

    public string? TweetText { get; set; }

    public string? Tags { get; set; }

    public ReplyType? ReplyType { get; set; }

    public List<Replies>? Replies { get; set; } = new List<Replies>();
}