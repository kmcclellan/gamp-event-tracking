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
        /// Adds parameters to be included in calls to
        /// <see cref="Collect(Action{ITrackingData}, CancellationToken)"/>.
        /// </summary>
        /// <remarks>
        /// These parameters will be merged with subsequent calls and calls to
        /// <see cref="ITrackingData.AddParameters(TrackingParameters)"/>.
        /// </remarks>
        /// <param name="parameters">The parameters to add.</param>
        /// <returns>The same client instance for chaining.</returns>
        ITrackingClient AddParameters(TrackingParameters parameters);

        /// <summary>
        /// Send tracking data to GAMP's /collect endpoint.
        /// </summary>
        /// <param name="configure">The delegate configuring the data to be sent.</param>
        /// <param name="cancellationToken">A token for cancelling the HTTP request.</param>
        /// <returns>The task representing the asychronous operation.</returns>
        Task Collect(Action<ITrackingData> configure, CancellationToken cancellationToken = default);
    }

    /// <inheritdoc cref="ITrackingClient"/>
    public class TrackingClient : ITrackingClient
    {
        private readonly HttpClient http;
        private readonly ITrackingBuilder builder;

        private readonly List<TrackingParameters> parameters = new List<TrackingParameters>();

        /// <summary>
        /// Creates a new tracking client.
        /// </summary>
        /// <remarks>
        /// The instance retains any parameters added with
        /// <see cref="ITrackingClient.AddParameters(TrackingParameters)"/>.
        /// </remarks>
        /// <param name="http">The underlying HTTP client to use for requests.</param>
        public TrackingClient(HttpClient http) : this(http, new TrackingBuilder()) { }

        internal TrackingClient(HttpClient http, ITrackingBuilder builder)
        {
            this.http = http;
            this.builder = builder;

            this.http.BaseAddress ??= new Uri("https://www.google-analytics.com/");
        }

        ITrackingClient ITrackingClient.AddParameters(TrackingParameters parameters)
        {
            this.parameters.Add(parameters);
            return this;
        }

        async Task ITrackingClient.Collect(Action<ITrackingData> configure, CancellationToken cancellationToken)
        {
            var payload = builder.Build(data =>
            {
                foreach (var p in parameters) data.AddParameters(p);
                configure(data);
            });

            var uri = new StringBuilder("/g/collect?")
                .AppendUriParameters(payload.Parameters)
                .ToString();

            var content = new StringBuilder()
                .JoinAppend("\n", payload.EventParameters, (sb, ep) => sb.AppendUriParameters(ep))
                .ToString();

            cancellationToken.ThrowIfCancellationRequested();
            var response = await http.PostAsync(uri, new StringContent(content), cancellationToken);
            response.EnsureSuccessStatusCode();
        }
    }  
}
