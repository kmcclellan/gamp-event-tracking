using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Gamp.Tests.Mocks
{
    class MockHttp
    {
        public HttpClient Client { get; }
        public HttpMethod Method => handler.Invocations.Single().Method;
        public Uri Uri => handler.Invocations.Single().Uri;
        public HttpRequestHeaders Headers => handler.Invocations.Single().Headers;
        public byte[] Content => handler.Invocations.Single().Content;

        private readonly MockHandler handler = new MockHandler();

        public MockHttp()
        {
            Client = new HttpClient(handler);
        }

        private class MockHandler : HttpMessageHandler
        {
            public IReadOnlyCollection<Invocation> Invocations => invocations;

            private readonly List<Invocation> invocations = new List<Invocation>();

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var content = await request.Content.ReadAsByteArrayAsync();

                invocations.Add(new Invocation(request.Method, request.RequestUri, request.Headers, content));

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    RequestMessage = request
                };
            }

            public class Invocation
            {
                public HttpMethod Method { get; }
                public Uri Uri { get; }
                public HttpRequestHeaders Headers { get; }
                public byte[] Content { get; }

                public Invocation(HttpMethod method, Uri uri, HttpRequestHeaders headers, byte[] content)
                {
                    Method = method;
                    Uri = uri;
                    Headers = headers;
                    Content = content;
                }
            }
        }
    }
}
