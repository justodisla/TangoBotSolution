using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpClientLib
{
    public class HttpInspector
    {
        public static void InspectTransit(HttpResponseEvent value)
        {
            Console.WriteLine($"[Info] AccountObserverEvent Request: {value.Request}");
            var _content = value.Request.Content == null ? null : value.Request.Content.ReadAsStringAsync().Result;

            if (_content != null)
            {
                Console.WriteLine($"[Info] AccountObserverEvent Request Content: {_content}");
            }
            else
            {
                Console.WriteLine($"[Info] AccountObserverEvent Request Content: No Content");
            }

            Console.WriteLine($"[Info] AccountObserverEvent Response: {value.Response}");
            if (value != null && value.Response != null)
            {
                string responseContent = value.Response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"[Info] AccountObserverEvent Response Content: {responseContent}");
            }
        }
    }


}
