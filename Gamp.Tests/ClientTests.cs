using Gamp.Data;
using Gamp.Tests.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gamp.Tests
{
    [TestClass]
    public class ClientTests
    { 
        private readonly ITrackingClient client;

        private readonly MockHttp http = new MockHttp();
        private readonly MockTracking tracking = new MockTracking();

        public ClientTests()
        {
            client = new TrackingClient(http.Client, tracking.Collector);
        }

        [TestMethod]
        public async Task PostsToGoogle()
        {
            await client.Collect(_ => { });

            Assert.AreEqual(HttpMethod.Post, http.Method);
            Assert.AreEqual("www.google-analytics.com", http.Uri.Host);
        }

        [TestMethod]
        public async Task SendsParametersInQueryString()
        {
            await client.Collect(collection =>
                tracking.SetPayload(collection.Parameters, new Dictionary<string, string>
                {
                    { "param1", "val1"},
                    { "param2", "val2" }
                }));

            Assert.AreEqual("?param1=val1&param2=val2", http.Uri.Query);
        }

        [TestMethod]
        public async Task SendsUserAgentAsHeader()
        {
            await client.Collect(collection =>
                tracking.SetPayload(collection.Parameters, new Dictionary<string, string>
                {
                    { "ua", "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0" }
                }));

            Assert.AreEqual(
                "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0",
                http.Headers.UserAgent.ToString());
        }

        [TestMethod]
        public async Task IgnoresInvalidUserAgent()
        {
            await client.Collect(collection =>
                tracking.SetPayload(collection.Parameters, new Dictionary<string, string>
                {
                    { "ua", "Mozilla/5.0 ((Bad)" }
                }));

            Assert.AreEqual(0, http.Headers.UserAgent.Count);
        }

        [TestMethod]
        public async Task SendsEventsInBody()
        {
            await client.Collect(collection =>
            {
                tracking.SetPayload(collection.AddEvent(""), new Dictionary<string, string>
                {
                    { "p1", "value1" },
                    { "p2", "value2" },
                });
                tracking.SetPayload(collection.AddEvent(""), new Dictionary<string, string>
                {
                    { "p2", "value3" },
                    { "p3", "value4" },
                });
            });

            Assert.AreEqual(
                "p1=value1&p2=value2\np2=value3&p3=value4",
                Encoding.UTF8.GetString(http.Content));
        }

        [TestMethod]
        public async Task EscapesSpecialCharacters()
        {
            var parameters = new Dictionary<string, string>
            {
                { "1 + 1", "= 2" },
                { "a&b&c", "what is it?" }
            };

            await client.Collect(collection =>
            {
                tracking.SetPayload(collection.Parameters, parameters);
                tracking.SetPayload(collection.AddEvent(""), parameters);
            });

            foreach (var expected in new[]
            {
                "1%20%2B%201",
                "%3D%202",
                "a%26b%26c",
                "what%20is%20it%3F"
            })
            {
                StringAssert.Contains(http.Uri.Query, expected);
                StringAssert.Contains(Encoding.UTF8.GetString(http.Content), expected);
            }
        }

        [TestMethod]
        public async Task MergesClientParameters()
        {
            var parameters = default(ITrackingParameters?);

            await client.Collect(collection =>
            {
                parameters = tracking.GetParentParmeters(collection.Parameters);
            });

            Assert.IsNotNull(parameters);
            Assert.AreEqual(client.DefaultParameters, parameters);
        }

        [TestMethod]
        public void SetsApiVersion()
        {
            Assert.AreEqual(2, tracking.GetApiVersion(client.DefaultParameters));
        }

        [TestMethod]
        public async Task SetsEventName()
        {
            var ev1 = default(string);
            var ev2 = default(string);

            await client.Collect(collection =>
            {
                ev1 = tracking.GetEventName(collection.AddEvent("my-event"));
                ev2 = tracking.GetEventName(collection.AddEvent("another"));
            });

            Assert.AreEqual("my-event", ev1);
            Assert.AreEqual("another", ev2);
        }
    }
}
