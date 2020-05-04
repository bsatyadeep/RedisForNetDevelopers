using System;

namespace RedisConnectionDotNetCore
{
    public class Investigation
    {
        public int InvestigationId { get; set; }
        public int InvestigationStatus { get; set; }
        public DateTime DateTime { get; set; }

        public Investigation()
        {
        }

        public Investigation(int investigationId, int investigationStatus)
        {
            InvestigationId = investigationId;
            DateTime = DateTime.UtcNow;
            InvestigationStatus = investigationStatus;
        }
    }
}