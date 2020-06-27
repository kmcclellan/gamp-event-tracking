namespace Gamp.Data
{
    /// <summary>
    /// A contract for data sent to Google Analytics in a single HTTP request.
    /// </summary>
    public interface ITrackingCollection
    {
        /// <summary>
        /// Shared parameters applicable to all events in this collection.
        /// </summary>
        ITrackingParameters Parameters { get; }

        /// <summary>
        /// Adds a tracking event to be collected.
        /// </summary>
        /// <remarks>
        /// Multiple events can be added to a collection.
        /// </remarks>
        /// <param name="name">The name of the event.</param>
        /// <returns>The event parameters to be modified.</returns>
        ITrackingParameters AddEvent(string name);
    }
}
