using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamp.Tests
{
    [TestClass]
    public class CollectorTests
    {
        private readonly TrackingCollector tracker = new TrackingCollector();

        [TestMethod]
        public void SetsStandardParameters()
        {
            var payload = tracker.Begin();
            payload.AddApiVersion(2)
                .AddTrackingId("G-0123456789")
                .AddClientId("gamp-tests")
                .FirstVisit()
                .AddSessionId("12345")
                .AddSessionNumber(74)
                .SessionStart()
                .SessionEngaged()
                .AddSessionHits(25)
                .AddDocumentLocation("https://test.com")
                .AddDocumentReferrer("https://other.com")
                .AddDocumentTitle("Hello world!")
                .AddScreenResolution("100x200")
                .AddUserLanguage("fr-CA")
                .AddUserAgent("PostmanRuntime/7.25.0");

            CollectionAssert.IsSubsetOf(new Dictionary<string, string>
            {
                { "v", "2" },
                { "tid", "G-0123456789" },
                { "cid", "gamp-tests" },
                { "_fv", "1" },
                { "sid", "12345" },
                { "sct", "74" },
                { "_ss", "1" },
                { "seg", "1" },
                { "_s", "25" },
                { "dl", "https://test.com" },
                { "dr", "https://other.com" },
                { "dt", "Hello world!" },
                { "sr", "100x200" },
                { "ul", "fr-CA" },
                { "ua", "PostmanRuntime/7.25.0" }
            }, payload.ToArray());
        }

        [TestMethod]
        public void SetsCustomParameters()
        {
            var payload = tracker.Begin();
            payload.AddEventName("my-event")
                .AddEventTime(TimeSpan.FromSeconds(30))
                .AddEventParameter("p1", "val1")
                .AddEventParameter("p2", "val2")
                .AddEventParameter("num", 99.87)
                .AddUserParameter("age", 43)
                .AddUserParameter("shirt", "green");        

            CollectionAssert.IsSubsetOf(new Dictionary<string, string>
            {
                { "en", "my-event" },
                { "_et", "30000" },
                { "ep.p1", "val1" },
                { "ep.p2", "val2" },
                { "epn.num", "99.87" },
                { "upn.age", "43" },
                { "up.shirt", "green" }
            }, payload.ToArray());
        }

        [TestMethod]
        public void MergesPayloads()
        {
            var payload1 = tracker.Begin();
            payload1.AddTrackingId("G-1111111111")
                .AddClientId("client1");

            var payload2 = tracker.Begin(payload1);
            payload2.AddClientId("client2")
                .AddSessionId("session");

            CollectionAssert.IsSubsetOf(new Dictionary<string, string>
            {
                { "tid", "G-1111111111" },
                { "cid", "client2" },
                { "sid", "session" }
            }, payload2.ToArray());
        }

    }
}
