using Microsoft.Extensions.Configuration;

namespace RedisConnectionDotNetCoreTest
{
    public class TestConfig
    {
        public static IConfiguration GetTestConfig()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            return config;
        }
    }
}