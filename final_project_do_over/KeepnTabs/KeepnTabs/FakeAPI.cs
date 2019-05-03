using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace KeepnTabs
{
    public class FakeAPI : HttpMessageHandler
    {
        protected override Task< HttpResponseMessage > SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
        {
            var segs = request.RequestUri.Segments;

            return Task.FromResult(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent( "OK" )
                }
            );
        }
    }
}
