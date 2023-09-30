using System.Collections.Concurrent;
﻿using Newtonsoft.Json;
using System.Collections.Concurrent;
using TwitterCloneBackend.Entities.enums;
using TwitterCloneBackend.Entities.Models;
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

        // Simulate distribution data via the message broker.
        Task t1 = Task.Factory.StartNew(() =>
        {
            tweet.TryAdd(id, RepoData.RepoData.AddOperations1());
            Thread.Sleep(3000);
            task = Tweet(tweet);

            // remove the tweet from concurrent collection after submission.
            //removed = RemoveTweet(tweet);
        });

        Task t2 = Task.Factory.StartNew(() =>
        {
            tweet.TryAdd(id, RepoData.RepoData.AddOperations2());
            tweet.TryAdd(id, RepoData.RepoData.AddOperations3());
            Thread.Sleep(5000);
            task = Tweet(tweet);

            // remove the tweet from concurrent collection after submission.
            //removed = RemoveTweet(tweet);
        });

        Task t3 = Task.Factory.StartNew(() =>
        {
            tweet.TryAdd(id, RepoData.RepoData.AddOperations4());
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
}