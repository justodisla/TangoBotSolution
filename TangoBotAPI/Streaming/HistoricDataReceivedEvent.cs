using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotAPI.Streaming
{
    public class HistoricDataReceivedEvent
    {
        public HistoricDataReceivedEvent(string receivedData)
        {
            ReceivedData = receivedData;
        }

        public string ReceivedData { get; }
    }
}
