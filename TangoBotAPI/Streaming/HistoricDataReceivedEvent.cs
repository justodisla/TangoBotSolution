using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBot.API.Streaming
{
    public class HistoricDataReceivedEvent
    {
        public WsResponse.DataItem ReceivedData { get; }
        public HistoricDataReceivedEvent(WsResponse.DataItem receivedData)
        {
            ReceivedData = receivedData;
        }
    }
}
