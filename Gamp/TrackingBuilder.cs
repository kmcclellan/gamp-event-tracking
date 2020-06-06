using Gamp.Data;
using System;
using System.Collections.Generic;

namespace Gamp
{
    interface ITrackingBuilder
    {
        ITrackingPayload Build(Action<ITrackingData> configure);
    }

    class TrackingBuilder : ITrackingBuilder
    {
        public ITrackingPayload Build(Action<ITrackingData> configure)
        {
            var data = new TrackingData();
            configure(data);
            return data;
        }

        private class TrackingData : Dictionary<string, string>, ITrackingData, ITrackingPayload
        {
            public IReadOnlyDictionary<string, string> Parameters => this;
            public IReadOnlyCollection<IReadOnlyDictionary<string, string>> EventParameters => events;

            private readonly List<IReadOnlyDictionary<string, string>> events
                = new List<IReadOnlyDictionary<string, string>>();

            public TrackingData()
            {
                this["v"] = "2";
            }

            public ITrackingData AddParameters(TrackingParameters parameters)
            {
                AddParameter("tid", parameters.TrackingId);
                AddParameter("cid", parameters.ClientId);
                AddParameter("_fv", parameters.FirstVisit);
                AddParameter("sid", parameters.SessionId);
                AddParameter("sct", parameters.SessionNumber);
                AddParameter("_ss", parameters.SessionStart);
                AddParameter("seg", parameters.SessionEngagement);
                AddParameter("_s", parameters.SessionHits);
                AddParameter("dl", parameters.DocumentLocation);
                AddParameter("dr", parameters.DocumentReferrer);
                AddParameter("dt", parameters.DocumentTitle);
                AddParameter("sr", parameters.ScreenResolution);
                AddParameter("ul", parameters.UserLanguage);
                return this;
            }

            public ITrackingData AddEvent(string name, Action<ITrackingEvent>? configure = null)
            {
                var @event = new TrackingEvent
                {
                    { "en", name }
                };
                configure?.Invoke(@event);
                events.Add(@event);
                return this;
            }

            private void AddParameter(string name, string? value)
            {
                if (value != null) this[name] = value;
            }

            private void AddParameter(string name, int? value)
            {
                if (value != null) this[name] = value.ToString();
            }

            private void AddParameter(string name, bool value)
            {
                if (value) this[name] = "1";
            }

            private class TrackingEvent : Dictionary<string, string>, ITrackingEvent
            {
                public ITrackingEvent AddTime(TimeSpan time)
                {
                    this["_et"] = time.TotalMilliseconds.ToString();
                    return this;
                }

                public ITrackingEvent AddParameter(string name, string value)
                {
                    this[$"ep.{name}"] = value;
                    return this;
                }

                public ITrackingEvent AddParameter(string name, double value)
                {
                    this[$"epn.{name}"] = value.ToString();
                    return this;
                }

                public ITrackingEvent AddUserParameter(string name, string value)
                {
                    this[$"up.{name}"] = value;
                    return this;
                }

                public ITrackingEvent AddUserParameter(string name, double value)
                {
                    this[$"upn.{name}"] = value.ToString();
                    return this;
                }
            }
        }
    }
}
