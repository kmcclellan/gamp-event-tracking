using System;

namespace Gamp.Data
{
    /// <summary>
    /// A contract for the kinds of tracking data supported by Google Analytics.
    /// </summary>
    public interface ITrackingParameters
    {
        /// <summary>
        /// Adds the tracking/measurement ID for the property data stream.
        /// </summary>
        /// <remarks>
        /// The format is G-XXXXXXXXXX.
        /// </remarks>
        /// <param name="value">The tracking ID.</param>
        ITrackingParameters AddTrackingId(string value);

        /// <summary>
        /// Adds a unique and anonymous identifier for a particular user, device, or browser instance.
        /// </summary>
        /// <param name="value">The client ID.</param>
        ITrackingParameters AddClientId(string value);

        /// <summary>
        /// Indicates that this is the client's first recorded interaction.
        /// </summary>
        ITrackingParameters FirstVisit();

        /// <summary>
        /// Adds an identifier for the current client session.
        /// </summary>
        /// <param name="value">The session ID.</param>
        ITrackingParameters AddSessionId(string value);

        /// <summary>
        /// Adds the total number of recorded sessions.
        /// </summary>
        /// <param name="value">The number of sessions.</param>
        ITrackingParameters AddSessionNumber(int value);

        /// <summary>
        /// Indicates that is the start of a new session.
        /// </summary>
        ITrackingParameters SessionStart();

        /// <summary>
        /// Indicates that user has engaged/continued the session.
        /// </summary>
        ITrackingParameters SessionEngaged();

        /// <summary>
        /// Adds the number of hits/events recorded in this session.
        /// </summary>
        /// <param name="value">The number of hits.</param>
        ITrackingParameters AddSessionHits(int value);

        /// <summary>
        /// Adds the location (URL) of the current page/document.
        /// </summary>
        /// <param name="value">The document location.</param>
        ITrackingParameters AddDocumentLocation(string value);

        /// <summary>
        /// Adds the location (URL) of the referring page/document.
        /// </summary>
        /// <param name="value">The document referrer.</param>
        ITrackingParameters AddDocumentReferrer(string value);

        /// <summary>
        /// Adds the title of the current page/document.
        /// </summary>
        /// <param name="value">The document title.</param>
        ITrackingParameters AddDocumentTitle(string value);

        /// <summary>
        /// Adds the pixel dimensions of the user's screen.
        /// </summary>
        /// <remarks>
        /// The format is two numbers joined by an 'x' (e.g. "400x800").
        /// </remarks>
        /// <param name="value">The screen resolution.</param>
        ITrackingParameters AddScreenResolution(string value);

        /// <summary>
        /// Adds the user's preferred language (as an IETF language tag).
        /// </summary>
        /// <param name="value">The user language.</param>
        ITrackingParameters AddUserLanguage(string value);

        /// <summary>
        /// Adds the HTTP User-Agent header associated with the user.
        /// </summary>
        /// <param name="value">The user agent.</param>
        ITrackingParameters AddUserAgent(string value);

        /// <summary>
        /// Adds the time the event occurred (relative to session start).
        /// </summary>
        /// <param name="time">The event time.</param>
        /// <returns>The same instance for chaining.</returns>
        ITrackingParameters AddEventTime(TimeSpan time);

        /// <summary>
        /// Adds a custom text parameter associated with an event.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The text value.</param>
        /// <returns>The same instance for chaining.</returns>
        ITrackingParameters AddEventParameter(string name, string value);

        /// <summary>
        /// Adds a custom numeric parameter associated with an event.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The numeric value.</param>
        /// <returns>The same instance for chaining.</returns>
        ITrackingParameters AddEventParameter(string name, double value);

        /// <summary>
        /// Adds a custom text parameter associated with the user.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The text value.</param>
        /// <returns>The same instance for chaining.</returns>
        ITrackingParameters AddUserParameter(string name, string value);

        /// <summary>
        /// Adds a custom numeric parameter associated with the user.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The numeric value.</param>
        /// <returns>The same instance for chaining.</returns>
        ITrackingParameters AddUserParameter(string name, double value);
    }
}
