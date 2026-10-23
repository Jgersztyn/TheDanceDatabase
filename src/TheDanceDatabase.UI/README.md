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
- **Line endings:** `.gitattributes` keeps files as LF on checkout, so `format:check` passes on Windows even with `core.autocrlf=true`.

For type-aware lint rules, replace `tseslint.configs.recommended` with `tseslint.configs.recommendedTypeChecked` and point the parser at the project's tsconfig files. See the [typescript-eslint docs](https://typescript-eslint.io/getting-started/typed-linting).

## VS Code

Open this folder (`src/TheDanceDatabase.UI`) as the workspace and install the recommended **ESLint** and **Prettier** extensions (VS Code prompts for them via `.vscode/extensions.json`). `.vscode/settings.json` formats on save with Prettier and applies ESLint fixes on save.
