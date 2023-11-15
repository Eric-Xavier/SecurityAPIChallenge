using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class DummyHttpMessageHandler : HttpMessageHandler
    {
        public DummyHttpMessageHandler()
        {
            HttpResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("{\"ISIN\":\"ABCDEFGHIJKL\" ,\"price\" : 10}")
            };
        }

        public HttpResponseMessage HttpResponse { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(HttpResponse);
        }
    }
}
