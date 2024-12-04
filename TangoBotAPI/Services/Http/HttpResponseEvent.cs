using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangoBot.API.Http
{
    public class HttpResponseEvent
    {
        public HttpRequestMessage? Request { get; set; }
        public HttpResponseMessage? Response { get; set; }
        public Exception? HttpException { get; set; }

        public HttpResponseEvent(HttpRequestMessage? request, HttpResponseMessage? response, Exception? exception)
        {
            Request = request;
            Response = response;
            HttpException = exception;
        }

    }
}