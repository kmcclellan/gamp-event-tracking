namespace Gamp.Data
{
    /// <summary>
    /// A model for standard parameters expected by Google Analytics.
    /// </summary>
    /// <remarks>
    /// <c>null</c> or <c>false</c> values will be ignored.
    /// </remarks>
    public class TrackingParameters
    {
        /// <summary>
        /// The tracking/measurement ID for the property data stream.
        /// </summary>
        /// <remarks>
        /// The format is G-XXXXXXXXXX.
        /// </remarks>
        public string? TrackingId { get; set; }

        /// <summary>
        /// A unique and anonymous identifier for a particular user, device, or browser instance.
        /// </summary>
        public string? ClientId { get; set; }

        /// <summary>
        /// Whether this is the client's first recorded interaction.
        /// </summary>
        public bool FirstVisit { get; set; }

        /// <summary>
        /// An identifier for the current client session.
        /// </summary>
        public string? SessionId { get; set; }

        /// <summary>
        /// The total number of recorded sessions.
        /// </summary>
        public int? SessionNumber { get; set; }

        /// <summary>
        /// Whether this is the start of a new session.
        /// </summary>
        public bool SessionStart { get; set; }

        /// <summary>
        /// Whether the user has engaged/continued the session.
        /// </summary>
        public bool SessionEngagement { get; set; }

        /// <summary>
        /// The number of events recorded in this session.
        /// </summary>
        public int? SessionHits { get; set; }

        /// <summary>
        /// The location (URL) of the current page/document.
        /// </summary>
        public string? DocumentLocation { get; set; }

        /// <summary>
        /// The location (URL) of the referring page/document.
        /// </summary>
        public string? DocumentReferrer { get; set; }

        /// <summary>
        /// The title of the current page/document.
        /// </summary>
        public string? DocumentTitle { get; set; }

        /// <summary>
        /// The pixel dimensions of the user's screen.
        /// </summary>
        /// <remarks>
        /// The format is two numbers joined by an 'x' (e.g. "400x800").
        /// </remarks>
        public string? ScreenResolution { get; set; }

        /// <summary>
        /// The user's preferred language (as an IETF language tag).
        /// </summary>
        public string? UserLanguage { get; set; }

        /// <summary>
        /// The HTTP User-Agent header associated with the user.
        /// </summary>
        public string? UserAgent { get; set; }
    }
}
