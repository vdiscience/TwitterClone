using TwitterCloneClient.Producer.Producer;

namespace TwitterCloneClient.Producer;

public abstract class Program
{
    public static void Main()
    {
        var publishTasks = new[]
        {
            Task.Run(() => Publish(RepoData.RepoData.AddOperations1())),
            Task.Run(() => Publish(RepoData.RepoData.AddOperations2())),
            Task.Run(() => Publish(RepoData.RepoData.AddOperations3())),
            Task.Run(() => Publish(RepoData.RepoData.AddOperations4()))
        };

        Task.WaitAll(publishTasks);
    }

    private static void Publish(string json)
    {
        ProducerHandler.Producer(json);
    }
}