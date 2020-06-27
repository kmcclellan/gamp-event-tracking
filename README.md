# GAMP Event Tracking
Client for the event-based (v2) Google Analytics Measurement Protocol.

### Features
* Manually send tracking data for Google Analytics App + Web Properties.
* Easily build GAMP requests using fluent syntax.
* Compatible with `Microsoft.Extensions.*`

## Installation
Add the NuGet package to your project:

    $ dotnet add package gamp-event-tracking

## Usage

Register client using `Microsoft.Extensions.Http`:
```c#
services.AddHttpClient<ITrackingClient, TrackingClient>();
```

Send an event to GAMP using `ITrackingClient`:
```c#
await client.Collect(collection =>
{
    collection.Parameters
        .AddTrackingId("G-XXXXXXXXXX")
        .AddClientId("gamp-readme");

    collection.AddEvent("test")
        .AddEventParameter("hello", "world!");
});
```
## Documentation

Since App + Web is still in Beta, there isn't any documentation for the tracking data supported by the v2 GAMP API. The parameters exposed on this API are subject to change, and their exact meaning is uncertain.

What information I've discovered is provided via XML/intellisense documentation. These resources may also be useful:

* [App + Web documentation](https://developers.google.com/analytics/devguides/collection/app-web)
* [GAMP v1 documentation](https://developers.google.com/analytics/devguides/collection/protocol/v1): beware, much has changed!
* [This article](https://www.thyngster.com/app-web-google-analytics-measurement-protocol-version-2) by David Vallejo