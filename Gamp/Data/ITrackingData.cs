using System;

namespace Gamp.Data
{
    /// <summary>
    /// A model for the data sent to Google Analytics.
    /// </summary>
    public interface ITrackingData
    {
        /// <summary>
        /// Adds standard tracking parameters to be sent.
        /// </summary>
        /// <remarks>
        /// These parameters apply to all added events. They will be merged with subsequent calls.
        /// </remarks>
        /// <param name="parameters">The parameters to add.</param>
        /// <returns>The same data instance for chaining.</returns>
        ITrackingData AddParameters(TrackingParameters parameters);

        /// <summary>
        /// Adds a tracking event to be sent.
        /// </summary>
        /// <remarks>
        /// Multiple events can be added and will be sent together.
        /// </remarks>
        /// <param name="name">The name of the event.</param>
        /// <param name="configure">A delegate configuring the event data.</param>
        /// <returns>The same data instance for chaining.</returns>
        ITrackingData AddEvent(string name, Action<ITrackingEvent>? configure = null);
    }
}
