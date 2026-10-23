# The Dance Database

The Dance Database is a collection of known dance styles, techniques and individual moves. More to come later!

## API

The API is an ASP.NET Core minimal API on .NET 10. `src/TheDanceDatabase.API` is the .NET app's root folder:

```
src/TheDanceDatabase.API/
├── TheDanceDatabase.API.slnx
├── global.json, Directory.Build.props, Directory.Packages.props, .editorconfig
├── TheDanceDatabase.API/              # the API project
└── TheDanceDatabase.API.UnitTests/    # its tests
```

Run the `dotnet` commands from `src/TheDanceDatabase.API`, so the SDK pinned in `global.json` is used:

```
dotnet run --project TheDanceDatabase.API   # http://localhost:5185, API reference at /scalar
dotnet test --solution TheDanceDatabase.API.slnx
```

The API project's code is organized in layers:

- `Application/`: service interfaces (`I*Service`) and inbound request models
- `Domain/`: entities, enums and configuration classes (`Configuration/`)
- `Infrastructure/`: the implementations of the `Application` interfaces
- `Presentation/`: route mapping (`Endpoints.cs`) and the handler bodies (`Functions.cs`)

Put machine-specific settings in `appsettings.Local.json` next to `appsettings.json`. That file is git-ignored and never copied to build or publish output. It is read only in the Development environment, and environment variables and command-line arguments still override it.