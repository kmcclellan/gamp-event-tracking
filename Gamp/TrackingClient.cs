using Gamp.Data;
using Gamp.Extensions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gamp
{
    /// <summary>
    /// An HTTP client for sending tracking data to Google Analytics
    /// using the Measurement Protocol.
    /// </summary>
    public interface ITrackingClient
    {
        /// <summary>
        /// Static parameters to be included in calls to
        /// <see cref="Collect(Action{ITrackingCollection}, CancellationToken)"/>.
        /// </summary>
        /// <remarks>
        /// This data will be merged with <see cref="ITrackingCollection.Parameters"/>.
        /// </remarks>
        ITrackingParameters DefaultParameters { get; }

        /// <summary>
        /// Send tracking data to GAMP's /collect endpoint.
        /// </summary>
        /// <param name="configure">A delegate configuring the data to be collected.</param>
        /// <param name="cancellationToken">A token for cancelling the HTTP request.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Collect(Action<ITrackingCollection> configure, CancellationToken cancellationToken = default);
    }

    /// <inheritdoc cref="ITrackingClient"/>
    public class TrackingClient : ITrackingClient
    {
        ITrackingParameters ITrackingClient.DefaultParameters => payload;

        private readonly HttpClient http;
        private readonly ITrackingCollector collector;

        private readonly ITrackingPayload payload;

        /// <summary>
        /// Creates a new tracking client.
        /// </summary>
        /// <param name="http">The underlying HTTP client to use for requests.</param>
        public TrackingClient(HttpClient http) : this(http, new TrackingCollector()) { }

        internal TrackingClient(HttpClient http, ITrackingCollector collector)
        {
            this.http = http;
            this.collector = collector;
            payload = collector.Begin()
                .AddApiVersion(2);

            this.http.BaseAddress ??= new Uri("https://www.google-analytics.com/");
        }

        async Task ITrackingClient.Collect(Action<ITrackingCollection> collect, CancellationToken cancellationToken)
        {
            var collection = new TrackingCollection(collector, payload);
            collect(collection);

            var uri = new StringBuilder("/g/collect?")
                .AppendUriParameters(collection.SharedPayload)
                .ToString();

            var content = new StringBuilder()
                .JoinAppend("\n", collection.EventPayload, (sb, p) => sb.AppendUriParameters(p))
                .ToString();

            using var request = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(content)
            };

            if (collection.SharedPayload.TryGetValue("ua", out var userAgent))
            {
                request.Headers.TryAddWithoutValidation("User-Agent", userAgent);
            }

            cancellationToken.ThrowIfCancellationRequested();
            var response = await http.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
        }

        private class TrackingCollection : ITrackingCollection
        {
            public ITrackingParameters Parameters => SharedPayload;

            public ITrackingPayload SharedPayload { get; }

            public IReadOnlyCollection<ITrackingPayload> EventPayload => events;

            private readonly ITrackingCollector collector;
            private readonly List<ITrackingPayload> events = new List<ITrackingPayload>();

            public TrackingCollection(ITrackingCollector collector, ITrackingPayload payload)
            {
                this.collector = collector;
                SharedPayload = collector.Begin(payload);
            }

            public ITrackingParameters AddEvent(string name)
            {
                var eventData = collector.Begin()
                    .AddEventName(name);

                events.Add(eventData);
                return eventData;
            }
        }
    }  
}
