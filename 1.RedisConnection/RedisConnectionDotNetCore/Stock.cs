using System;

namespace RedisConnectionDotNetCore
{
    public class Stock
    {
        public Stock(int stockId, int stockStatus)
        {
            StockId = stockId;
            StockStatus = stockStatus;
            DateTime = DateTime.UtcNow;

        }
        public int StockId { get; set; }
        public int StockStatus { get; set; }
        public DateTime DateTime { get; set; }
    }
}