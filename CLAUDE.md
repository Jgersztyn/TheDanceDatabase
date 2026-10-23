# CLAUDE.md

The Dance Database is a collection of known dance styles, techniques and individual moves. It has a React UI and a .NET API, and both run in Docker.

## Repository layout

The repo uses a container layout: each app has its own folder under `src/`, with its projects side by side.

| Path | What it is | Edited in |
| --- | --- | --- |
| `src/TheDanceDatabase.API/` | .NET app root: `TheDanceDatabase.API.slnx`, `global.json`, `Directory.Build.props`, `Directory.Packages.props`, `.editorconfig`, `.dockerignore` | Visual Studio or Rider |
| `src/TheDanceDatabase.API/TheDanceDatabase.API/` | .NET 10 ASP.NET Core minimal API | Visual Studio or Rider |
| `src/TheDanceDatabase.API/TheDanceDatabase.API.UnitTests/` | API tests (xUnit v3 + Shouldly) | Visual Studio or Rider |
| `src/TheDanceDatabase.UI/` | UI container folder: `.vscode/`, `.gitattributes`, `.dockerignore` | VS Code (open this folder) |
| `src/TheDanceDatabase.UI/TheDanceDatabase.UI/` | React 19 + TypeScript + Vite app | VS Code |
| `src/TheDanceDatabase.UI/TheDanceDatabase.UI.ComponentTests/` | UI component tests (Vitest + React Testing Library). An empty placeholder until the first test is written | VS Code |
| `docker-compose.yml` | Runs the API and the UI in containers | |

## Working rules

- **Keep UI and API work on separate branches.** A UI branch does not touch `src/TheDanceDatabase.API`, and an API branch does not touch `src/TheDanceDatabase.UI`. Repo-wide work, such as Docker or layout changes, may touch both.
- **C# and .NET code follows the `jg-code-style` skill.** Read `C:\Repos\claude-personal-files\skills\jg-code-style\SKILL.md` before you write, review or restructure any `.cs`, `.csproj` or solution file. It is not an installed skill, so open the file directly. It does not govern the UI.
- **Project names use dotted PascalCase:** `TheDanceDatabase.<Part>`. Older notes may call the UI `TheDanceDatabase.Client`, but the folder is `TheDanceDatabase.UI`.
- **Nothing app-specific goes at the repo root.** Build files belong in each app's container folder. Only repo-wide files, such as `docker-compose.yml`, live at the root.
- **Branches:** recent ones are named `task/<snake_case>` or `feature/<snake_case>`. PRs target `main`.
- **CI/CD will run through Octopus.** No Octopus project exists yet.
- **Scaffolding template:** this repo is the container-layout reference for `../claude-personal-files/project-templates/new-project-scaffolding.md`, relative to the repo root. That file is a template for future projects, not a guide for this one. When this repo's layout, tooling or Docker setup changes, update the template to match.

## UI (`src/TheDanceDatabase.UI`)

Run these from `src/TheDanceDatabase.UI/TheDanceDatabase.UI` after running `npm install` once:

```
npm run dev            # dev server with hot reload, http://localhost:5173
npm run build          # tsc -b type-check, then vite build to dist/
npm run lint           # ESLint
npm run format         # Prettier, rewrites files
npm run format:check   # Prettier, reports only (the CI form)
```

- **TypeScript only.** Source files are `.ts`/`.tsx`. The only JavaScript file is `eslint.config.js`.
- **Linting is ESLint, not Oxlint.** create-vite defaults to Oxlint unless you pass `--eslint`. This project chose ESLint, so don't add Oxlint back. `eslint-config-prettier` must stay the last entry in `eslint.config.js`.
- **Prettier owns formatting.** It is pinned to an exact version, and its settings are in `.prettierrc`. Before finishing a UI change, run `npm run format`, then `npm run lint` and `npm run build`.
- **Line endings are LF.** `src/TheDanceDatabase.UI/.gitattributes` forces it for the app and the tests, because this machine has `core.autocrlf=true` and Prettier fails on CRLF.
- **VS Code settings** (format on save, ESLint fix on save) live in `src/TheDanceDatabase.UI/.vscode/`. They apply only when that container folder is opened as the workspace. `eslint.workingDirectories` is set to `auto`, so ESLint runs from the app folder.
- **Component tests use Vitest and React Testing Library.** The tests go in `TheDanceDatabase.UI.ComponentTests/`, which holds only an empty `App.test.ts` for now. The app has these packages as dev dependencies: `vitest`, `@testing-library/react`, `@testing-library/dom`, `@testing-library/jest-dom` and `jsdom`.
  - **Not wired up yet:** there is no Vitest config, no `test` script and no real test. The test folder has no lint, format or type-check setup, and it is left out of the Docker build context.
  - **When you wire it up:** the tests sit outside the app folder, so the Vitest config needs `server.fs.allow: ['..']`. It also needs `resolve.dedupe`, listing `react`, `react-dom`, `@testing-library/react`, `@testing-library/jest-dom` and `vitest`. Without both, the test file isn't loaded, and its bare imports don't resolve to the app's `node_modules`. A throwaway RTL test of `App` passed with that config, along with `environment: 'jsdom'` and the React plugin (checked 2026-10-23).
  - **Then** add the `test` script, and update this file and the app README.

## API (`src/TheDanceDatabase.API`)

Run these from `src/TheDanceDatabase.API`, so the SDK pin and the test runner setting in `global.json` apply:

```
dotnet run --project TheDanceDatabase.API   # http://localhost:5185; Scalar API reference at /scalar (Development only)
dotnet test --solution TheDanceDatabase.API.slnx
```

- **Layers:**
  - `Application/` holds the `I*Service` interfaces and request models.
  - `Domain/` holds entities, enums and `Configuration/` POCOs.
  - `Infrastructure/` holds the implementations of the `Application` interfaces.
  - `Presentation/` holds `Endpoints.cs` (route mapping) and the `Functions` static class (handler bodies).
- **Package versions** are managed centrally in `Directory.Packages.props`. Project files reference packages without a version.
- **Machine-specific settings** go in `appsettings.Local.json`. It is git-ignored, never copied to build or publish output, kept out of the Docker build context, and read only in Development.

## Docker

Run these from the repo root:

```
docker compose up --build   # API at http://localhost:5185, UI at http://localhost:8080
docker compose down
```

- **Each app's container folder is its build context.** Each Dockerfile sits in the project it builds:
  - `src/TheDanceDatabase.API/TheDanceDatabase.API/Dockerfile`. The `.dockerignore` is in `src/TheDanceDatabase.API/`.
  - `src/TheDanceDatabase.UI/TheDanceDatabase.UI/Dockerfile`, next to `nginx.conf`. The `.dockerignore` is in `src/TheDanceDatabase.UI/`.
- **API image:** built on `dotnet/sdk:10.0`, run on `dotnet/aspnet:10.0` as the non-root `app` user, listening on 8080. Restore copies `global.json` and both `Directory.*.props` files first.
- **UI image:** built on `node:24-alpine`, served by `nginx-unprivileged:1.30-alpine` on 8080. Unknown paths fall back to `index.html`, and a missing file under `/assets/` returns 404.
- **Build one image** from its context folder, e.g. from `src/TheDanceDatabase.API`: `docker build -f TheDanceDatabase.API/Dockerfile .` (`-f` is relative to the current folder, not the context).
- **Compose** binds both ports to `127.0.0.1` and runs the API in Development, so Scalar and the debug logging are on. It adds the UI container's origin as `CorsConfig__AllowedOrigins__1`, so index 0 stays the Vite dev server from `appsettings.Development.json`.
- **Ports:** the API container uses 5185, the same port as `dotnet run`. Stop one before you start the other.
