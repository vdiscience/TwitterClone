using Apache.NMS;
using Apache.NMS.Util;
using System.Text.Json;
using Newtonsoft.Json;
using TwitterCloneBackend.DDD;
using TwitterCloneBackend.DDD.enums;
using TwitterCloneBackend.DDD.Models;
using TwitterCloneClient.Producer.Producer;

namespace TwitterCloneClient.Producer;

public class Program
{
    public static void Main()
    {
        ProducerHandler.Producer(AddOperations());

        string AddOperations()
        {
            var root = new User
            {
                UserName = "vancho",
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
            //    WriteIndented = truek
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