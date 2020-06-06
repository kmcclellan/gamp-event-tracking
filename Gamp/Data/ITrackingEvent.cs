using System;

namespace Gamp.Data
{
    /// <summary>
    /// A model for data representing a user event.
    /// </summary>
    /// <remarks>
    /// This data is entirely customizable, though there are some standard events
    /// that Google Analytics expects to have certain parameters (e.g. page_view).
    /// </remarks>
    public interface ITrackingEvent
    {
        /// <summary>
        /// Adds the time the event occurred (relative to session start).
        /// </summary>
        /// <param name="time">The event time.</param>
        /// <returns>The same event instance for chaining.</returns>
        ITrackingEvent AddTime(TimeSpan time);

        /// <summary>
        /// Adds a custom text parameter associated with the event.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The text value.</param>
        /// <returns>The same event instance for chaining.</returns>
        ITrackingEvent AddParameter(string name, string value);

        /// <summary>
        /// Adds a custom numeric parameter associated with the event.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The numeric value.</param>
        /// <returns>The same event instance for chaining.</returns>
        ITrackingEvent AddParameter(string name, double value);

        /// <summary>
        /// Adds a custom text parameter associated with the user.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The text value.</param>
        /// <returns>The same event instance for chaining.</returns>
        ITrackingEvent AddUserParameter(string name, string value);

        /// <summary>
        /// Adds a custom numeric parameter associated with the user.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The numeric value.</param>
        /// <returns>The same event instance for chaining.</returns>
        ITrackingEvent AddUserParameter(string name, double value);
    }
}
