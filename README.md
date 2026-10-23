# The Dance Database

A website that hosts custom made simfiles for StepMania and other rhythm games.

[Learn more about StepMania here](https://www.stepmania.com/)

## API

The API is an ASP.NET Core minimal API on .NET 10, in `src/TheDanceDatabase.API`. Its tests are in `tests/TheDanceDatabase.API.UnitTests`.

```
dotnet run --project src/TheDanceDatabase.API   # http://localhost:5185, API reference at /scalar
dotnet test --solution TheDanceDatabase.slnx
```

The code is organized in layers:

- `Application/`: service interfaces (`I*Service`) and inbound request models
- `Domain/`: entities, enums and configuration classes (`Configuration/`)
- `Infrastructure/`: the implementations of the `Application` interfaces
- `Presentation/`: route mapping (`Endpoints.cs`) and the handler bodies (`Functions.cs`)

Put machine-specific settings in `appsettings.Local.json` next to `appsettings.json`. That file is git-ignored and never copied to build or publish output. It is read only in the Development environment, and environment variables and command-line arguments still override it.