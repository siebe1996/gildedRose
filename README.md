# Gilded Rose (Full Refactor)

This repository is a full refactor of the Gilded Rose kata. The original kata was created by Terry Hughes ([twitter](http://twitter.com/TerryHughes)) and is available on [GitHub](https://github.com/NotMyself/GildedRose). See also [Bobby Johnson's description of the kata](http://iamnotmyself.com/2011/02/13/refactor-this-the-gilded-rose-kata/).

## Overview

The solution contains the following parts:

- **Api**: a fake API that serves the items. It uses internal in-memory collections instead of a database, so no database setup is needed.
- **Kiota-generated clients**: the Windows service and the console app use a typed HTTP client generated with [Kiota](https://learn.microsoft.com/openapi/kiota/) from the API's OpenAPI description.
- **WindowsService**: a Windows service (`BackgroundService`) that runs an update job on a schedule. The schedule is set in `appsettings.json`.
- **GildedRose.App**: a console application that uses the generated Kiota client to interact with the API.
- **Aspire AppHost** (`GildedRose.AppHost`, placeholder name): orchestrates the API, the Windows service, and the console app so they start and run together.

## Prerequisites

- .NET 10 SDK
- Visual Studio 2026 with the .NET Aspire and Windows service workloads

The required tools (Kiota and the Aspire CLI) are defined in `.config/dotnet-tools.json`. Restore them with:

```
dotnet tool restore
```

## Running the solution

1. Open `GildedRose.sln` in Visual Studio.
2. Set the Aspire AppHost project (`GildedRose.AppHost`, placeholder) as the startup project.
3. Press __F5__ to start the API, the Windows service, and the console app together.

## OpenAPI specification

The API publishes its OpenAPI specification while it runs in the `Development` environment:

- HTTPS: `https://localhost:7146/openapi/v1.json`
- HTTP: `http://localhost:5175/openapi/v1.json`

## Regenerating the Kiota client

The generated client is used by both the **WindowsService** and **GildedRose.App** projects. Regenerate it whenever the API contract changes.

1. Run `dotnet tool restore` if you haven't already.
2. Make sure the API is running, or point Kiota at its OpenAPI document (placeholder: `Api/openapi.json`).
3. Run the following from the repository root in PowerShell. The paths and names are placeholders, so adjust them to match your project:

```
# Regenerate the client for the Windows service
dotnet kiota generate -l CSharp -d https://localhost:7146/openapi/v1.json -o .\WindowsService\ApiClient -n WindowsService.ApiClient -c GildedRoseApiClient

# Regenerate the client for the console app
dotnet kiota generate -l CSharp -d https://localhost:7146/openapi/v1.json -o .\GildedRose.App\ApiClient -n GildedRose.App.ApiClient -c GildedRoseApiClient
```

Kiota stores its settings in `kiota-lock.json`. Commit this file so that regeneration stays consistent across machines.

## Configuring the update job schedule (WindowsService)

The update job interval is defined in `WindowsService\appsettings.json`:

```json
{
  "Job": {
    "Interval": "00:00:15"
  }
}
```

- `Interval`: how often the job runs, as a .NET `TimeSpan` string (`hh:mm:ss`). The example `00:00:15` runs the job every 15 seconds. Use `00:01:00` for every minute or `1.00:00:00` for every day.

Restart the Windows service after changing the file so the new interval is applied.

## Running the tests

Run the tests from __Test Explorer__ in Visual Studio, or from the terminal:

```
dotnet test GildedRose.sln
```