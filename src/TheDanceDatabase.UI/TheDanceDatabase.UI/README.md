# React + TypeScript + Vite

This template provides a minimal setup to get React working in Vite with HMR, linted by ESLint and formatted by Prettier.

Currently, two official plugins are available:

- [@vitejs/plugin-react](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react) uses [Oxc](https://oxc.rs)
- [@vitejs/plugin-react-swc](https://github.com/vitejs/vite-plugin-react/blob/main/packages/plugin-react-swc) uses [SWC](https://swc.rs/)

## React Compiler

The React Compiler is not enabled on this template because of its impact on dev & build performances. To add it, see [this documentation](https://react.dev/learn/react-compiler/installation).

## Scripts

Run `npm install` once, then:

| Command                | What it does                                        |
| ---------------------- | --------------------------------------------------- |
| `npm run dev`          | Start the dev server with hot reload                |
| `npm run build`        | Type-check (`tsc -b`) and build to `dist/`          |
| `npm run preview`      | Serve the production build locally                  |
| `npm run lint`         | Lint with ESLint                                    |
| `npm run format`       | Format every file with Prettier                     |
| `npm run format:check` | Report files Prettier would change (use this in CI) |

## Linting and formatting

- **ESLint** (`eslint.config.js`) checks `.ts`/`.tsx` files with the recommended rules from `@eslint/js`, `typescript-eslint`, `eslint-plugin-react-hooks` and `eslint-plugin-react-refresh`. `eslint-config-prettier` is the last entry so ESLint never reports formatting; keep it last when adding configs.
- **Prettier** (`.prettierrc`, `.prettierignore`) owns formatting. It is pinned to an exact version because even minor releases can change its output.
- **Line endings:** `.gitattributes` in the UI container folder (`src/TheDanceDatabase.UI`) keeps files as LF on checkout, so `format:check` passes on Windows even with `core.autocrlf=true`.

For type-aware lint rules, replace `tseslint.configs.recommended` with `tseslint.configs.recommendedTypeChecked` and point the parser at the project's tsconfig files. See the [typescript-eslint docs](https://typescript-eslint.io/getting-started/typed-linting).

## VS Code

Open the UI container folder (`src/TheDanceDatabase.UI`, one level up) as the workspace, so the app and `TheDanceDatabase.UI.ComponentTests` are both in view. Install the recommended **ESLint** and **Prettier** extensions (VS Code prompts for them via `.vscode/extensions.json`). `.vscode/settings.json` formats on save with Prettier, applies ESLint fixes on save, and lets ESLint find this folder's config (`eslint.workingDirectories`).

## Testing

Component tests use [Vitest](https://vitest.dev) with [React Testing Library](https://testing-library.com/docs/react-testing-library/intro/), `@testing-library/jest-dom` matchers and the `jsdom` environment. The packages are installed as dev dependencies. Nothing is wired up yet: there is no Vitest config, no `test` script and no test. The tests will live in `../TheDanceDatabase.UI.ComponentTests`, next to this app. Because that folder is outside the app, the Vitest config needs `server.fs.allow: ['..']`, and `resolve.dedupe` for `react`, `react-dom` and the test packages, so the tests resolve this app's `node_modules`.

## Docker

`Dockerfile` builds the app on Node and serves `dist/` with nginx (`nginx.conf`) on port 8080. Its build context is the container folder. Run the UI and the API together from the repo root with `docker compose up --build`.
