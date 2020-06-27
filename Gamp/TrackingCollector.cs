using Gamp.Data;
using System;
using System.Collections.Generic;

namespace Gamp
{
    interface ITrackingCollector
    {
        ITrackingPayload Begin(ITrackingPayload? payload = null);
    }

    interface ITrackingPayload : IReadOnlyDictionary<string, string>, ITrackingParameters
    {
        ITrackingPayload AddApiVersion(int value);
        ITrackingPayload AddEventName(string value);
    }

    class TrackingCollector : ITrackingCollector
    {
        public ITrackingPayload Begin(ITrackingPayload? payload = null)
        {
            var data = new TrackingPayload();
            if (payload != null)
            {
                foreach (var kvp in payload)
                {
                    data[kvp.Key] = kvp.Value;
                }
            }

            return data;
        }

        private class TrackingPayload : Dictionary<string, string>, ITrackingPayload
        {
            public ITrackingParameters Data => this;

            public ITrackingParameters AddTrackingId(string value) =>
                AddParameter("tid", value);

            public ITrackingParameters AddClientId(string value) =>
                AddParameter("cid", value);

            public ITrackingParameters FirstVisit() =>
                AddParameter("_fv");

            public ITrackingParameters AddSessionId(string value) =>
                AddParameter("sid", value);

            public ITrackingParameters AddSessionNumber(int value) =>
                AddParameter("sct", value);

            public ITrackingParameters SessionStart() =>
                AddParameter("_ss");

            public ITrackingParameters SessionEngaged() =>
                AddParameter("seg");

            public ITrackingParameters AddSessionHits(int value) =>
                AddParameter("_s", value);

            public ITrackingParameters AddDocumentLocation(string value) =>
                AddParameter("dl", value);

            public ITrackingParameters AddDocumentReferrer(string value) =>
                AddParameter("dr", value);

            public ITrackingParameters AddDocumentTitle(string value) =>
                AddParameter("dt", value);

            public ITrackingParameters AddScreenResolution(string value) =>
                AddParameter("sr", value);

            public ITrackingParameters AddUserLanguage(string value) =>
                AddParameter("ul", value);

            public ITrackingParameters AddUserAgent(string value) =>
                AddParameter("ua", value);

            public ITrackingParameters AddEventTime(TimeSpan time) =>
                AddParameter("_et", time.TotalMilliseconds);

            public ITrackingParameters AddEventParameter(string name, string value) =>
                AddParameter($"ep.{name}", value);

            public ITrackingParameters AddEventParameter(string name, double value) =>
                AddParameter($"epn.{name}", value);

            public ITrackingParameters AddUserParameter(string name, string value) =>
                AddParameter($"up.{name}", value);

            public ITrackingParameters AddUserParameter(string name, double value) =>
                AddParameter($"upn.{name}", value);

            public ITrackingPayload AddApiVersion(int value)
            {
                this["v"] = value.ToString();
                return this;
            }

            public ITrackingPayload AddEventName(string value)
            {
                this["en"] = value;
                return this;
            }

            private ITrackingParameters AddParameter(string name, string value)
            {
                this[name] = value;
                return this;
            }

            private ITrackingParameters AddParameter(string name, double value)
            {
                this[name] = value.ToString();
                return this;
            }

            private ITrackingParameters AddParameter(string name)
            {
                this[name] = "1";
                return this;
            }
        }
    }
}
