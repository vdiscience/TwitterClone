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
        ProducerHandler.Producer();
    }
}