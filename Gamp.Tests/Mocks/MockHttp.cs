using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Gamp.Tests.Mocks
{
    class MockHttp : HttpMessageHandler
    {
        public HttpClient Client { get; }

        public HttpStatusCode StatusCode { private get; set; }

        private HttpRequestMessage? capturedRequest;
        private string? capturedContent;

        public MockHttp()
        {
            Client = new HttpClient(this);
        }

        public void VerifyMethod(HttpMethod method) =>
            Assert.AreEqual(method, capturedRequest?.Method);

        public void VerifyHeaders(Predicate<HttpRequestHeaders> condition)
        {
            Assert.IsNotNull(capturedRequest?.Headers);
            Assert.IsTrue(condition(capturedRequest!.Headers));
        }

        public void VerifyUri(Predicate<Uri> condition)
        {
            Assert.IsNotNull(capturedRequest?.RequestUri);
            Assert.IsTrue(condition(capturedRequest!.RequestUri));
        }

        public void VerifyContent(Predicate<string> condition)
        {
            Assert.IsNotNull(capturedContent);
            Assert.IsTrue(condition(capturedContent!));
        }

        protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
        {
            capturedRequest = request;
            if (request.Content != null)
            {
                capturedContent = await request.Content.ReadAsStringAsync();
            }
            return new HttpResponseMessage(StatusCode);
        }
    }
}
