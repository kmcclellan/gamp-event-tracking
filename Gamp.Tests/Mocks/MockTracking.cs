using Gamp.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gamp.Tests.Mocks
{
    class MockTracking
    {
        public ITrackingCollector Collector => collector;

        private readonly MockCollector collector = new MockCollector();

        public void SetPayload(
            ITrackingParameters parameters,
            IEnumerable<KeyValuePair<string, string>> payload)
        {
            var mock = GetInvocation(parameters).Returned;

            foreach (var kvp in payload)
            {
                mock[kvp.Key] = kvp.Value;
            }    
        }

        public ITrackingParameters? GetParentParmeters(ITrackingParameters parameters) =>
            GetInvocation(parameters).Captured;

        public int? GetApiVersion(ITrackingParameters parameters) =>
            GetInvocation(parameters).Returned.ApiVersion;

        public string? GetEventName(ITrackingParameters parameters) =>
            GetInvocation(parameters).Returned.EventName;


        private MockCollector.Invocation GetInvocation(ITrackingParameters parameters)
        {
            return collector.Invocations.Single(i => i.Returned == parameters);
        }

        private class MockCollector : Dictionary<string, string>, ITrackingCollector
        {
            public IReadOnlyCollection<Invocation> Invocations => invocations;

            private readonly List<Invocation> invocations = new List<Invocation>();


            public ITrackingPayload Begin(ITrackingPayload? payload = null)
            {
                var result = new MockPayload();
                invocations.Add(new Invocation(payload, result));
                return result;
            }

            public class Invocation
            {
                public ITrackingPayload? Captured { get; }
                public MockPayload Returned { get; }

                public Invocation(ITrackingPayload? captured, MockPayload returned)
                {
                    Captured = captured;
                    Returned = returned;
                }
            }
        }

        private class MockPayload : Dictionary<string, string>, ITrackingPayload
        {
            public int? ApiVersion { get; private set; }
            public string? EventName { get; private set; }

            public ITrackingPayload AddApiVersion(int value)
            {
                ApiVersion = value;
                return this;
            }

            public ITrackingPayload AddEventName(string value)
            {
                EventName = value;
                return this;
            }

            public ITrackingParameters AddClientId(string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddDocumentLocation(string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddDocumentReferrer(string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddDocumentTitle(string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddEventParameter(string name, string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddEventParameter(string name, double value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddEventTime(TimeSpan time)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddScreenResolution(string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddSessionHits(int value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddSessionId(string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddSessionNumber(int value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddTrackingId(string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddUserAgent(string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddUserLanguage(string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddUserParameter(string name, string value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters AddUserParameter(string name, double value)
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters FirstVisit()
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters SessionEngaged()
            {
                throw new NotImplementedException();
            }

            public ITrackingParameters SessionStart()
            {
                throw new NotImplementedException();
            }
        }
    }
}
