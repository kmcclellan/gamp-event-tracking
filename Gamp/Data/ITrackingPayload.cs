using System.Collections.Generic;

namespace Gamp.Data
{
    interface ITrackingPayload
    {
        IReadOnlyDictionary<string, string> Parameters { get; }
        IReadOnlyCollection<IReadOnlyDictionary<string, string>> EventParameters { get; }
    }
}
