using Gamp.Data;
using Gamp.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gamp.Tests
{
    [TestClass]
    public class TrackingClientTests
    { 
        private readonly ITrackingClient client;

        private readonly MockHttp http = new MockHttp() { StatusCode = HttpStatusCode.OK };
        private readonly MockTracker tracker = new MockTracker();

        public TrackingClientTests()
        {
            client = new TrackingClient(http.Client, tracker);
        }

        [TestMethod]
        public async Task PostsToGoogle()
        {
            await client.Collect(_ => { });

            http.VerifyMethod(HttpMethod.Post);
            http.VerifyUri(uri => uri.Host == "www.google-analytics.com");
        }

        [TestMethod]
        public async Task SendsParametersInQueryString()
        {
            tracker.Parameters = new Dictionary<string, string>
            {
                { "param1", "val1"},
                { "param2", "val2" }
            };

            await client.Collect(_ => { });

            http.VerifyUri(uri => uri.Query == "?param1=val1&param2=val2");
        }

        [TestMethod]
        public async Task SendsEventsInBody()
        {
            tracker.EventParameters = new[]
            {
                new Dictionary<string, string>
                {
                    { "name", "e1" },
                    { "param1", "val1" },
                },
                new Dictionary<string, string>
                {
                    { "name", "e2" },
                    { "param2", "val2" },
                }
            };

            await client.Collect(_ => { });

            http.VerifyContent(content => content == "name=e1&param1=val1\nname=e2&param2=val2");
        }

        [TestMethod]
        public async Task EscapesSpecialCharacters()
        {
            var parameters = new Dictionary<string, string>
            {
                { "1 + 1", "= 2" },
                { "a&b&c", "what is it?" }
            };
            tracker.Parameters = parameters;
            tracker.EventParameters = new[] { parameters };

            await client.Collect(_ => { });

            foreach (var expected in new[]
            {
                "1%20%2B%201",
                "%3D%202",
                "a%26b%26c",
                "what%20is%20it%3F"
            })
            {
                http.VerifyUri(uri => uri.Query.Contains(expected));
                http.VerifyContent(content => content.Contains(expected));
            }
        }

        [TestMethod]
        public async Task CallsConfigureWithData()
        {
            object? captured = null;

            await client.Collect(data => captured = data);
            tracker.CapturedConfigure!.Invoke(tracker);

            Assert.IsNotNull(captured);
        }

        [TestMethod]
        public async Task PassesParametersBeforeConfigure()
        {
            var parameters = Enumerable.Range(0, 2)
                .Select(_ => new TrackingParameters()).ToArray();
            foreach (var p in parameters) client.AddParameters(p);
            bool addedOnConfigure = false;

            await client.Collect(td => addedOnConfigure = tracker.CapturedParameters.Any());
            tracker.CapturedConfigure!.Invoke(tracker);

            CollectionAssert.AreEqual(parameters, tracker.CapturedParameters);
            Assert.IsTrue(addedOnConfigure);
        }
    }
}
