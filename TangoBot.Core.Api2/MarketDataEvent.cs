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

        public MarketDataEvent(Guid id, DataPoint dataPoint)
        {
            Id = id;
            DataPoint = dataPoint;
        }

    }
}
