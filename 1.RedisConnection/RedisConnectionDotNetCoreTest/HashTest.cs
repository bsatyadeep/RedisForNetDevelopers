using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using StackExchange.Redis;

namespace RedisConnectionDotNetCoreTest
{
    [TestClass]
    public class HashTest
    {
        private static IDatabase _redis;
        [ClassInitialize]
        public void Setup()
        {
            var testConfig = TestConfig.GetTestConfig();
            Check.That(testConfig["redis.connection"]).IsNotEmpty();

            var config = ConfigurationOptions.Parse(testConfig["redis.connection"]);
            config.AllowAdmin = true;

            var cm = ConnectionMultiplexer.Connect(config);
            var server = cm.GetServer(testConfig["redis.connection"]);
            server.FlushDatabase();

            _redis = cm.GetDatabase();
        }
        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
