using HttpClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBotAPI.Streaming;

namespace TangoBotStreaming.Observables
{
    public class HistoryDataStreamObserver : IObserver<CandleEvent>
    {

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(CandleEvent value)
        {
            Console.WriteLine("\n\n------------\n\n");
            Console.WriteLine("HistoryDataStreamObserver: " + value);
            Console.WriteLine("\n\n------------\n\n");
        }
    }
}
