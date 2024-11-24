using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLib.OrderApi.Observer
{
    public class OrderObserver : IObserver<HttpResponseEvent>
    {
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(HttpResponseEvent value)
        {
            HttpInspector.InspectTransit(value);
        }

        
    }
}
