using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotAPI.Streaming
{
    public class HistoricDataReceivedEvent
    {
        public HistoricDataReceivedEvent(string v)
        {
            V = v;
        }

        public string V { get; }
    }
}
