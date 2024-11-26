using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotAPI.Streaming
{
    public class HistoricDataReceivedEvent
    {
        public string ReceivedData { get; }
        public HistoricDataReceivedEvent(string receivedData)
        {
            ReceivedData = receivedData;
        }
    }
}
