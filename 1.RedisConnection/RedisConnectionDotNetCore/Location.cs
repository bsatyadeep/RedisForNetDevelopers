using System;

namespace RedisConnectionDotNetCore
{
    public class Location
    {
        public Location()
        {
            
        }
        public Location(int locationId, int locationStatus)
        {
            LocationId = locationId;
            LocationStatus = locationStatus;
            DateTime = DateTime.UtcNow;
            Stock = new Stock(locationId,locationStatus);
            Capacity=new Capacity(locationId,locationStatus);
        }

        public int LocationId { get; set; }
        public int LocationStatus { get; set; }
        public DateTime DateTime { get; set; }
        public Stock Stock { get; set; }
        public Capacity Capacity { get; set; }
    }
}