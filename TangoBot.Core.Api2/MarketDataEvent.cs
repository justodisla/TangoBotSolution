using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotApi.Common;

namespace TangoBot.Core.Api2
{
    public class MarketDataEvent : IEvent
    {

        public Guid Id { get; }

        public DataPoint DataPoint { get; set; }

        public string Comment { get; set; }

        public string Subject { get; set; }

        public MarketDataEvent(string subject, DataPoint dataPoint, string comment = "")
        {   
            DataPoint = dataPoint;
            Comment = comment;
            Subject = subject;
        }

    }
}
