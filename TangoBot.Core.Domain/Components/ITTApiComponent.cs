using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangoBot.API.Http;

namespace TangoBot.Core.Domain.Components
{
    public interface ITTApiComponent
    {
        Task<HttpResponseMessage?> SendRequestAsync(string endPoint, HttpMethod method, HttpContent? content = null);
        IDisposable Subscribe(IObserver<HttpResponseEvent> observer);
    }
}
