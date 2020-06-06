using Gamp.Data;
using System;
using System.Collections.Generic;

namespace Gamp.Tests.Mocks
{
    class MockTracker : ITrackingBuilder, ITrackingData, ITrackingPayload
    {
        public IReadOnlyDictionary<string, string> Parameters { get; set; } =
            new Dictionary<string, string>();

        public IReadOnlyCollection<IReadOnlyDictionary<string, string>> EventParameters { get; set; } =
            new List<IReadOnlyDictionary<string, string>>();

        public List<TrackingParameters> CapturedParameters { get; } = new List<TrackingParameters>();

        public Action<ITrackingData>? CapturedConfigure { get; private set; }

        public ITrackingPayload Build(Action<ITrackingData> configure)
        {
            CapturedConfigure = configure;
            return this;
        }

        public ITrackingData AddParameters(TrackingParameters parameters)
        {
            CapturedParameters.Add(parameters);
            return this;
        }

        public ITrackingData AddEvent(string name, Action<ITrackingEvent>? configure = null)
        {
            throw new NotImplementedException();
        }
    }
}
