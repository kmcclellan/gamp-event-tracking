using Gamp.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamp.Tests
{
    [TestClass]
    public class TrackingBuilderTests
    {
        private readonly TrackingBuilder builder = new TrackingBuilder();

        [TestMethod]
        public void SetsParameters()
        {
            var payload = builder.Build(d => d
                .AddParameters(new TrackingParameters
                {
                    TrackingId = "G-0123456789",
                    ClientId = "gamp-tests",
                    FirstVisit = true,
                    SessionId = "12345",
                    SessionNumber = 74,
                    SessionStart = true,
                    SessionEngagement = true,
                    SessionHits = 25,
                    DocumentLocation = "https://test.com",
                    DocumentReferrer = "https://other.com",
                    DocumentTitle = "Hello world!",
                    ScreenResolution = "100x200",
                    UserLanguage = "fr-CA"
                }));

            foreach (var (key, value) in new Dictionary<string, string>
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
                { "ul", "fr-CA" }
            })
            {
                Assert.AreEqual(value, payload.Parameters[key]);
            }
        }

        [TestMethod]
        public void SetsEventParameters()
        {
            var payload = builder.Build(d => d
                .AddEvent("e1")
                .AddEvent("e2", e => e
                    .AddParameter("p1", "val1")
                    .AddParameter("p2", "val2")
                    .AddUserParameter("age", 43))
                .AddEvent("e3", e => e
                    .AddTime(TimeSpan.FromSeconds(30))
                    .AddParameter("num", 99.87)
                    .AddUserParameter("shirt", "green")));

            foreach (var expected in new[]
            {
                new Dictionary<string, string>
                {
                    { "en", "e1" }
                },
                new Dictionary<string, string>
                {
                    { "en", "e2" },
                    { "ep.p1", "val1" },
                    { "ep.p2", "val2" },
                    { "upn.age", "43" }
                },
                new Dictionary<string, string>
                {
                    { "en", "e3" },
                    { "_et", "30000" },
                    { "epn.num", "99.87" },
                    { "up.shirt", "green" }
                }
            })
            {
                Assert.IsTrue(payload.EventParameters.Any(ep => expected.All(kvp => ep[kvp.Key] == kvp.Value)));
            }
        }

        [TestMethod]
        public void MergesMultipleParameters()
        {
            var payload = builder.Build(d => d
                .AddParameters(new TrackingParameters
                {
                    TrackingId = "G-1111111111",
                    ClientId = "client1"
                }).AddParameters(new TrackingParameters
                {
                    ClientId = "client2",
                    SessionId = "session"
                }));

            foreach (var (key, value) in new Dictionary<string, string>
            {
                { "tid", "G-1111111111" },
                { "cid", "client2" },
                { "sid", "session" }
            })
            {
                Assert.AreEqual(value, payload.Parameters[key]);
            }
        }
    }
}
