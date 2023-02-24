using Newtonsoft.Json;
using System.Collections.Concurrent;
using TwitterCloneBackend.DDD.enums;
using TwitterCloneBackend.DDD.Models;
using TwitterCloneClient.Producer.Producer;

namespace TwitterCloneClient.Producer;

public abstract class Program
{
    // Simulation of concurrency. Concurrent tweet action.
    private static ConcurrentDictionary<Guid, string> tweet = new ConcurrentDictionary<Guid, string>();

    public static void Main()
    {
        Guid id;
        id = Guid.NewGuid();
        Task task;
        Task<bool> removed;

        Task t1 = Task.Factory.StartNew(() =>
        {
            tweet.TryAdd(id, AddOperations1().ToString());
            Thread.Sleep(3000);
            task = Tweet(tweet);

            // remove the tweet from concurrent collection after submission.
            //removed = RemoveTweet(tweet);
        });

        Task t2 = Task.Factory.StartNew(() =>
        {
            tweet.TryAdd(id, AddOperations2().ToString());
            tweet.TryAdd(id, AddOperations3().ToString());
            Thread.Sleep(5000);
            task = Tweet(tweet);

            // remove the tweet from concurrent collection after submission.
            //removed = RemoveTweet(tweet);
        });

        Task t3 = Task.Factory.StartNew(() =>
        {
            tweet.TryAdd(id, AddOperations4().ToString());
            Thread.Sleep(8000);
            task = Tweet(tweet);

            // remove the tweet from concurrent collection after submission.
            //removed = RemoveTweet(tweet);
        });


        try
        {
            Task.WaitAll(t1, t2, t3);
        }
        catch (AggregateException ex)
        {
            throw ex;
        }
    }

    public static Task<bool> RemoveTweet(ConcurrentDictionary<Guid, string> tweeted)
    {
        foreach (var tw in tweeted)
        {
            string removedItem;
            bool result = tweet.TryRemove(tw.Key, out removedItem); //Returns true
        }
        return (Task<bool>)Task.CompletedTask;
    }

    private static Task Tweet(ConcurrentDictionary<Guid, string> tweet)
    {
        foreach (var tw in tweet)
        {
            ProducerHandler.Producer(tw.Value);
        }

        return Task.CompletedTask;
    }

    static string AddOperations1()
    {
        var root = new User
        {
            UserName = "vancho1",
            Password = "ana",
            Profile = new Profile
            {
                ProfileName = "Vancho Dimitrov",
                Biography = "Being working as a software engineer for quite a while now.",
                Location = new Location
                {
                    City = new City
                    {
                        CityName = "London"
                    },
                    Country = new Country
                    {
                        CountryName = "United Kingdom"
                    }
                },
                Website = "https://angelchodimitrovski.com",
                BirthDate = DateTime.Now,
                Tweets = new List<Tweet>
                        {
                            new Tweet
                            {
                                MediaPath = "TextTweet",
                                TweetText = "Buy my book. Learn c# programming",
                                Tags = "#programming book",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/2.jpg",
                                TweetText = "See my image",
                                Tags = "#pussycat",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/dogs2.mpeg",
                                TweetText = "the lovliest dog ever.",
                                Tags = "#dog",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            }
                        }
            },
        };


        return JsonConvert.SerializeObject(root, Formatting.Indented, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        //return JsonSerializer.Serialize<User>(root, new JsonSerializerOptions()
        //{
        //    WriteIndented = true
        //});

        //return JsonConvert.SerializeObject(new
        //{
        //    root.UserName,
        //    root.Password,
        //    root.Profile.ProfileName,
        //    root.Profile.Biography,
        //    root.Profile.Location.City,
        //    root.Profile.Location.Country,
        //    root.Profile.Website,
        //    root.Profile.BirthDate,
        //    root.Profile.Tweets
        //}, Formatting.Indented,
        //    new JsonSerializerSettings()
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    });
    }

    static string AddOperations2()
    {
        var root = new User
        {
            UserName = "vancho2",
            Password = "ana",
            Profile = new Profile
            {
                ProfileName = "Vancho Dimitrov",
                Biography = "Being working as a software engineer for quite a while now.",
                Location = new Location
                {
                    City = new City
                    {
                        CityName = "London"
                    },
                    Country = new Country
                    {
                        CountryName = "United Kingdom"
                    }
                },
                Website = "https://angelchodimitrovski.com",
                BirthDate = DateTime.Now,
                Tweets = new List<Tweet>
                        {
                            new Tweet
                            {
                                MediaPath = "TextTweet",
                                TweetText = "Buy my book. Learn c# programming",
                                Tags = "#programmingbook",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/2.jpg",
                                TweetText = "See my image",
                                Tags = "#pussycat",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/dogs2.mpeg",
                                TweetText = "the lovliest dog ever.",
                                Tags = "#dog",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            }
                        }
            },
        };


        return JsonConvert.SerializeObject(root, Formatting.Indented, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        //return JsonSerializer.Serialize<User>(root, new JsonSerializerOptions()
        //{
        //    WriteIndented = true
        //});

        //return JsonConvert.SerializeObject(new
        //{
        //    root.UserName,
        //    root.Password,
        //    root.Profile.ProfileName,
        //    root.Profile.Biography,
        //    root.Profile.Location.City,
        //    root.Profile.Location.Country,
        //    root.Profile.Website,
        //    root.Profile.BirthDate,
        //    root.Profile.Tweets
        //}, Formatting.Indented,
        //    new JsonSerializerSettings()
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    });
    }

    static string AddOperations3()
    {
        var root = new User
        {
            UserName = "vancho3",
            Password = "ana",
            Profile = new Profile
            {
                ProfileName = "Vancho Dimitrov",
                Biography = "Being working as a software engineer for quite a while now.",
                Location = new Location
                {
                    City = new City
                    {
                        CityName = "London"
                    },
                    Country = new Country
                    {
                        CountryName = "United Kingdom"
                    }
                },
                Website = "https://angelchodimitrovski.com",
                BirthDate = DateTime.Now,
                Tweets = new List<Tweet>
                        {
                            new Tweet
                            {
                                MediaPath = "TextTweet",
                                TweetText = "Buy my book. Learn c# programming",
                                Tags = "#programmingbook",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/2.jpg",
                                TweetText = "See my image",
                                Tags = "#pussycat",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/dogs2.mpeg",
                                TweetText = "the lovliest dog ever.",
                                Tags = "#dog",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            }
                        }
            },
        };


        return JsonConvert.SerializeObject(root, Formatting.Indented, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        //return JsonSerializer.Serialize<User>(root, new JsonSerializerOptions()
        //{
        //    WriteIndented = true
        //});

        //return JsonConvert.SerializeObject(new
        //{
        //    root.UserName,
        //    root.Password,
        //    root.Profile.ProfileName,
        //    root.Profile.Biography,
        //    root.Profile.Location.City,
        //    root.Profile.Location.Country,
        //    root.Profile.Website,
        //    root.Profile.BirthDate,
        //    root.Profile.Tweets
        //}, Formatting.Indented,
        //    new JsonSerializerSettings()
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    });
    }

    static string AddOperations4()
    {
        var root = new User
        {
            UserName = "vancho4",
            Password = "ana",
            Profile = new Profile
            {
                ProfileName = "Vancho Dimitrov",
                Biography = "Being working as a software engineer for quite a while now.",
                Location = new Location
                {
                    City = new City
                    {
                        CityName = "London"
                    },
                    Country = new Country
                    {
                        CountryName = "United Kingdom"
                    }
                },
                Website = "https://angelchodimitrovski.com",
                BirthDate = DateTime.Now,
                Tweets = new List<Tweet>
                        {
                            new Tweet
                            {
                                MediaPath = "TextTweet",
                                TweetText = "Buy my book. Learn c# programming",
                                Tags = "#programmingbook",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/2.jpg",
                                TweetText = "See my image",
                                Tags = "#pussycat",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            },
                            new Tweet
                            {
                                MediaPath = "c:/dogs2.mpeg",
                                TweetText = "the lovliest dog ever.",
                                Tags = "#dog",
                                ReplyType = ReplyType.Everyone,
                                Replies = new List<Replies>
                                {
                                    new Replies
                                    {
                                        RepliedComment = "What a lovely book!"
                                    }
                                }
                            }
                        }
            },
        };


        return JsonConvert.SerializeObject(root, Formatting.Indented, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        //return JsonSerializer.Serialize<User>(root, new JsonSerializerOptions()
        //{
        //    WriteIndented = true
        //});

        //return JsonConvert.SerializeObject(new
        //{
        //    root.UserName,
        //    root.Password,
        //    root.Profile.ProfileName,
        //    root.Profile.Biography,
        //    root.Profile.Location.City,
        //    root.Profile.Location.Country,
        //    root.Profile.Website,
        //    root.Profile.BirthDate,
        //    root.Profile.Tweets
        //}, Formatting.Indented,
        //    new JsonSerializerSettings()
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    });
    }
}