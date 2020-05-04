using System;

namespace RedisConnectionDotNetCore
{
    public class Capacity
    {
        public Capacity(int capacityId, int capacityStatus)
        {
            CapacityId = capacityId;
            CapacityStatus = capacityStatus;
            DateTime = DateTime.UtcNow;
        }
        public int CapacityId { get; set; }
        public int CapacityStatus { get; set; }
        public DateTime DateTime { get; set; }
    }
}