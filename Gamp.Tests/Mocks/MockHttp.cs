using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Gamp.Tests.Mocks
{
    class MockHttp : HttpMessageHandler
    {
        public HttpClient Client { get; }

        public HttpStatusCode StatusCode { private get; set; }

        private HttpRequestMessage? capturedRequest;

        public MockHttp()
        {
            Client = new HttpClient(this);
        }

        public void VerifyMethod(HttpMethod method) =>
            Assert.AreEqual(method, capturedRequest!.Method);

        public void VerifyUri(Predicate<Uri> condition) =>
            Assert.IsTrue(condition(capturedRequest!.RequestUri));

        public void VerifyContent(Predicate<string> condition) =>
            Assert.IsTrue(condition(capturedRequest!.Content.ReadAsStringAsync().Result));

        protected override Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
        {
            capturedRequest = request;
            return Task.FromResult(new HttpResponseMessage(StatusCode));
        }
    }
}
