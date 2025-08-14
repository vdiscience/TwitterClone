using Newtonsoft.Json;
using TwitterCloneBackend.Entities.enums;
using TwitterCloneBackend.Entities.Models;

namespace TwitterCloneClient.Producer.RepoData
{
    public static class RepoData
    {
        // Simulate data generation.
        public static string AddOperations1()
        {
            var root = new User
            {
                UserName = "User 1",
                Password = "user1",
                Profile = new Profile
                {
                    ProfileName = "Profile 1",
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

        public static string AddOperations2()
        {
            var root = new User
            {
                UserName = "User 2",
                Password = "user2",
                Profile = new Profile
                {
                    ProfileName = "Profile 2",
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

        public static string AddOperations3()
        {
            var root = new User
            {
                UserName = "User 3",
                Password = "user3",
                Profile = new Profile
                {
                    ProfileName = "Profile 3",
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

        public static string AddOperations4()
        {
            var root = new User
            {
                UserName = "User 4",
                Password = "user4",
                Profile = new Profile
                {
                    ProfileName = "Profile 4",
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
}
