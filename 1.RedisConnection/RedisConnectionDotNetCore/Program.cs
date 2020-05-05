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

            var server = redis.Multiplexer.GetServer(config["redis.connection"]);

            server.FlushDatabase();

            #region MyRegion

            
            var baseKey = "GB#9404#050501380#";
            for (int i = 0; i <1000; i++)
            {
                var transaction = redis.CreateTransaction();

                var key = $"{baseKey}{i}-I";

                var investigationHash = (new Investigation(i, i)).ToHashEntries();
                //redis.HashSet(key, investigationHash);
                
                transaction.HashSetAsync(key, investigationHash);

                //redis.StringSet(key, $"Investigation_{i}");

                key = $"{baseKey}{i}-I#{i}-L";
                //HashEntry[] redisLocationHash = (new Location(i, i)).ToHashEntryList().ToArray();
                //redis.HashSet(key, redisLocationHash);

                Location location = new Location(i, i);
                //redis.HashSet(key, "Location", JsonConvert.SerializeObject(location));
                transaction.HashSetAsync(key, "Location", JsonConvert.SerializeObject(location));
                transaction.ExecuteAsync();
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

                var redisValue = redis.HashGet(redisKey, "Location");
                var location = JsonConvert.DeserializeObject<Location>(redisValue);
                Console.WriteLine($"LocationId:{location.LocationId}\tStockId:{location.Stock.StockId}\tCapacityId:{location.Capacity.CapacityId}");
            }

            var trans = redis.CreateTransaction();
            //redis.HashDelete(redisKeys.First(), "Location");
            var first = redisKeys.First();
            trans.AddCondition(Condition.HashExists(first,"Location"));
            trans.HashDeleteAsync(first, "Location");
            trans.ExecuteAsync();

            redisKeys = RedisStore.Connection.GetServer(config["redis.connection"]).Keys(pattern: keyToSearch);
            Console.WriteLine($"Total Locations Keys :{redisKeys.ToList().Count}");

            #endregion

            #region check if not deleted

            var hashAKey = "HashA";
            var hashBKey = "HashB";
            var id = "hashItemID";

            var tran = redis.CreateTransaction();

            // Only delete the item if it exists in both hashes
            var hashBCondition = tran.AddCondition(Condition.HashExists(hashBKey, id));
            var hashACondition = tran.AddCondition(Condition.HashExists(hashAKey, id));

            tran.HashDeleteAsync(hashBKey, id);
            tran.HashDeleteAsync(hashAKey, id);

            //bool deleted = await tran.ExecuteAsync();
            bool deleted =  tran.Execute();

            if (!deleted)
            {
                Console.WriteLine
                ("Failed to delete '{ID}'. HashAResult: {A}, HashBResult: {B}", id, hashACondition.WasSatisfied, hashBCondition.WasSatisfied);
            }

            #endregion
            Console.ReadKey();

        }
    }
}
