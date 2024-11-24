using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBotAPI.Streaming
{
    public class CandleEvent
    {
        public CandleEvent(string v)
        {
            V = v;
        }

        public string V { get; }
    }
}
