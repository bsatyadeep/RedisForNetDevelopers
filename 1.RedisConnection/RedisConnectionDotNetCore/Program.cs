using System;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisConnectionDotNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var redis = RedisStore.RedisCache;


            var baseKey = "GB#9404#050501380#";
            for (int i = 0; i <1000; i++)
            {
                var key = $"{baseKey}{i}-I";

                var investigationHash = (new Investigation(i, i)).ToHashEntries();
                redis.HashSet(key, investigationHash);

                //redis.StringSet(key, $"Investigation_{i}");

                key = $"{baseKey}{i}-I#{i}-L";
                //HashEntry[] redisLocationHash = (new Location(i, i)).ToHashEntries();
                Location location = new Location(i,i);
                //redis.HashSet(key, redisLocationHash);
                redis.HashSet(key, "Location",JsonConvert.SerializeObject(location));

                //redis.StringSet(key, $"Location{i}");
            }

            DateTime startDateTime = DateTime.Now;
            var keyToSearch = "GB#9404#050501380#*-I";
            var redisKeys = RedisStore.Connection.GetServer(config["redis.connection"]).Keys(pattern: keyToSearch);
            DateTime endDateTime = DateTime.Now;
            Console.WriteLine($"Total Investigated Keys :{redisKeys.ToList().Count}\t Total Time taken : {endDateTime-startDateTime}");

            //foreach (var redisKey in redisKeys)
            //{
            //    var hashEntries = redis.HashGetAll(redisKey).ToArray();
            //    var investigation = hashEntries.ConvertFromRedis<Investigation>();
            //    Console.WriteLine($"Key: {redisKey} Investigation: {JsonConvert.SerializeObject(investigation)}");
            //}

            startDateTime = DateTime.Now;
            keyToSearch = "GB#9404#050501380#*-I#*-L";
            redisKeys = RedisStore.Connection.GetServer(config["redis.connection"]).Keys(pattern: keyToSearch);
            endDateTime = DateTime.Now;
            Console.WriteLine($"Total Locations Keys :{redisKeys.ToList().Count}\t Total Time taken : {endDateTime - startDateTime}");

            foreach (var redisKey in redisKeys)
            {
                //var hashEntries = redis.HashGetAll(redisKey).ToArray();
                //var location = hashEntries.ConvertFromRedis<Location>();
                //Console.WriteLine($"Key: {redisKey} Location: {JsonConvert.SerializeObject(location)}");
                var redisValue = redis.HashGet(redisKey,"Location");
                var location = JsonConvert.DeserializeObject<Location>(redisValue);
                Console.WriteLine($"LocationId:{location.LocationId}\tStockId:{location.Stock.StockId}\tCapacityId:{location.Capacity.CapacityId}");
            }

            Console.ReadKey();

        }
    }
}
