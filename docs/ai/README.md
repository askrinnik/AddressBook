# AI harness of AddressBook

How the Claude Code environment of this repository is built: what it consists of, which parts come from plugins and which belong to the repository, and how to keep it healthy. The issue workflow comes from the `issue-kit` plugin and the harness benchmark and session profile from the `harness-kit` plugin, both of the `askrinnik/ai-plugins` marketplace. Everything specific to AddressBook — the stack rules, the project agents and skills, the build and test commands — stays in this repository.

## Layout

```
CLAUDE.md                              instruction hub
.mcp.json                              MCP servers (context7, microsoft-learn, nuget)
issue-kit.config.json                  this repository's values for the issue-kit workflow
profile-session.config.json            this repository's values for the harness-kit session profile
.ai/
  benchmarks/harness/                  harness benchmark: fixtures, history, reports
.claude/
  agents/*.md                          project agents
  rules/*.md                           path-scoped coding standards and documentation rules
  skills/                              project skills (`_local.*`) and generic on-demand skills
  settings.json                        plugins, permissions, attribution off, MCP servers enabled
docs/
  specs/                               specifications (API, Contracts, Web, Vue, Architecture)
  guides/                              learning guides (vue-for-blazor-developers.md, in Russian)
  tasks/                               one plan per issue: issue-<n>-<slug>.md
  ai/                                  this document
```

## Plugins

`.claude/settings.json` registers the marketplace (`extraKnownMarketplaces.ai-plugins`) and enables `issue-kit@ai-plugins` and `harness-kit@ai-plugins` for everyone who opens the repository.

| Plugin | What it gives | Repository file |
|---|---|---|
| `issue-kit` | The commands `/issue-kit:implement-issue`, `/issue-kit:implement-issues`, `/issue-kit:next-issue`; the skills `git-commit`, `open-pr`, `github-issue`, `init-config`; the agents `issue-planner`, `issue-developer`, `build-runner`, `skill-runner`; the commit message hook | `issue-kit.config.json` |
| `harness-kit` | The skills `profile-session` (cost and context of a finished session) and `harness-quality-check` (benchmark) | `profile-session.config.json`, `.ai/benchmarks/harness/fixtures/` |

The step trees and flowcharts of the two workflows, the configuration keys and the troubleshooting table are in the plugin: `docs/implement-issue.md`, `docs/implement-issues.md`, `CONFIGURATION.md` and `README.md` of `plugins/issue-kit`. Check the configuration with `/issue-kit:init-config check`.

## Workflow values of this repository

`issue-kit.config.json` holds only the values that differ from the plugin defaults:

| Key | Value | Why |
|---|---|---|
| `commands.build` | `dotnet build src/AddressBook.slnx -clp:ErrorsOnly` | The whole solution |
| `commands.testFull` | the bUnit suite `src/AddressBook.Web.Tests` | First of the four test runners |
| `commands.testExtra` | the Playwright API suite, the Playwright UI suite, the Vitest suite of `src/AddressBook.Vue` | The Playwright suites start the API, the Web app and the Vue app themselves |
| `commands.format` | `prettier --check` of `src/AddressBook.Vue` | The only format check of the repository |
| `plans.dir`, `plans.nameTemplate` | `docs/tasks`, `issue-{n}-{slug}.md` | The plan location that `.claude/rules/docs.md` defines |
| `lanes.testAuthoring` | `testing` | The test label of this repository |
| `roles.testAuthor` | `playwright-tester` | Writes Playwright specs with the Playwright MCP server in the Test-authoring lane |
| `roles.verifier` | `issue-verifier` | Reproduces a defect and walks the acceptance items in a real browser; the walk is required for items with visible behaviour |
| `roles.architect`, `roles.securityReviewer` | `architect`, `security-reviewer` | Project agents |
| `roles.debugSkill`, `roles.testsSkill` | `debug-issue`, `write-tests` | Project skills |
| `securityReview.triggers` | API controllers, validators, data access, `Program.cs`, configuration, CI workflows, package manifests | Changes that need the security review |
| `commentReview.globs` | `*.cs`, `*.razor`, `*.ts`, `*.vue` | Source files whose added comments the workflow checks |

The planner and the developer are the plugin agents. They do not have the `context7` and `microsoft-learn` MCP tools; if a plan or an implementation needs library documentation, the main session looks it up or a project agent is named in `roles.planner` or `roles.developer`.

## Project agents (`.claude/agents/`)

| Agent | Model | Role |
|---|---|---|
| `issue-verifier` | Sonnet | `roles.verifier`: browser walks produce large snapshots; the agent keeps them out of the main context and returns an evidence table |
| `playwright-tester` | Sonnet | `roles.testAuthor`: explores the UI with the Playwright MCP server and writes specs |
| `security-reviewer` | Opus | `roles.securityReviewer`: rare, and finding a real issue needs the strongest reasoning |
| `architect` | Opus | `roles.architect`: design questions on cross-layer changes |
| `software-engineer`, `csharp-dotnet-janitor`, `orchestrator`, `project-documenter` | per agent | Ad-hoc work outside the issue workflow |

The plugin agents take a prefix (`issue-kit:build-runner`); project agents have none.

## Rules (`.claude/rules/`)

Claude Code loads a rule when it works with a file that matches the rule's `paths:`, so a standard costs context only when it is needed.

| Rule | Paths | Content |
|---|---|---|
| `api-architecture.md` | `src/AddressBook.Api/**` | CQRS with MediatR, FluentValidation, RFC 7807, data access |
| `csharp.md` | `**/*.cs` | C# conventions |
| `blazor.md` | `*.razor`, `*.razor.cs`, `*.razor.css` | Blazor WebAssembly and MudBlazor conventions |
| `bunit.md` | `src/AddressBook.Web.Tests/**` | bUnit component tests |
| `playwright.md` | `src/ApiTests/**`, `src/UiTests/**` | Playwright API and UI E2E tests |
| `vue.md` | `src/AddressBook.Vue/**` | Vue, Vuetify and Vitest conventions |
| `github-actions.md` | `.github/workflows/**`, `.github/actions/**` | Workflow security and CI conventions |
| `docs.md` | `docs/**/*.md`, `**/README.md` | Where documents go, languages, plan format, style |
| `update-docs-on-code-change.md` | source, configuration and harness files | Which document each kind of change updates |

Directory-scoped `CLAUDE.md` files (`src/UiTests`, `src/AddressBook.Web.Tests`) add the non-negotiables of those suites.

## Skills (`.claude/skills/`)

Project skills (`_local.*`, invoked as `/<name>`):

| Skill | Purpose |
|---|---|
| `run-api`, `run-tests` | Start the API; run the Playwright API suite |
| `verify-feature` | Start the API and the Web app and walk acceptance items in a browser (used by `issue-verifier`) |
| `write-tests` | Pick the test layer (Playwright API / bUnit / Playwright UI) and write the tests |
| `debug-issue` | Reproduce and find the root cause of a defect |
| `refactor-code` | Behaviour-preserving refactoring |
| `nuget-package-update` | Pin-aware NuGet updates through `src/Directory.Packages.props`, family by family, with a build and test gate after each step |

Generic skills (`aspnet-core`, `ef-core`, `security-owasp`, `update-docs`, `code-review-checklist`, `dotnet-run-tests`, `test-anti-patterns`, `coverage-analysis`, `directory-build-organization`, …) load on demand. Rarely needed ones — `coverage-analysis`, `directory-build-organization`, `test-anti-patterns`, `create-specification`, `dotnet-timezone` — set `disable-model-invocation: true`: their descriptions stay out of the start context, and they run only when invoked as `/<name>`. `dotnet-run-tests` is the upstream `run-tests` skill, renamed because `_local.run-tests` owns that name. `document-workflow` writes or syncs the document of a workflow.

## MCP servers

| Server | Used for |
|---|---|
| `microsoft-learn` | Official .NET, ASP.NET Core, EF Core and Azure documentation and code samples (`microsoft-docs` skill) |
| `context7` | Documentation of third-party libraries: MudBlazor, FluentValidation, MediatR, bUnit, Playwright, Vue, Vue Router, Vuetify, Vitest |
| `nuget` | Package versions, vulnerabilities and release information (`nuget-package-update`); runs through `dnx`, which needs the .NET 10 SDK |

Claude Code asks each user once to approve the project servers in `.mcp.json`. GitHub work goes through `gh`.

## Permissions

`.claude/settings.json` is shared through git:

- **allow** — build, tests, running the API and the Web app, `git` and `gh` reads, the shipping commands (`git add`/`commit`/`push`, `gh pr create`, `gh issue create`/`comment`/`edit`), adding a "blocked by" relation and listing issues through `gh api`, the scripts of the `issue-kit` and `harness-kit` plugins and of `_local.nuget-package-update`, and the documentation MCP servers. Commit, push, PR and issue creation stay gated by instruction: the assistant does them only when the user asks (`CLAUDE.md`);
- **ask** — merging, PR comments and edits, closing issues, `gh api` calls with `-X DELETE`/`PATCH`/`PUT`, with `--method`, or with `-X` after the path, `dotnet ef database`. Other `gh api repos/…` calls match no rule, so Claude Code asks for them as for any command outside `allow`;
- **deny** — force-push, `reset --hard`, `git clean`, `rm -rf`, and reading `.env` or local `appsettings.*.local.json` secrets.

It also turns off the automatic commit and PR attribution (commit messages follow `issue-kit:git-commit`, and the plugin hook checks them) and enables the `.mcp.json` servers. Personal overrides go to `.claude/settings.local.json`, which git ignores.

## Harness benchmark

`/harness-kit:harness-quality-check` starts fixed `claude -p` sessions and records the start context, the tokens a file read adds, the rules loaded, and a judge's score against a ground truth. The fixtures, the history and the reports stay in this repository under `.ai/benchmarks/harness/`: `bench-base` (start context by area) and `quality-vertical-slice` (a feature request answered with the full slice, validation, tests and open decisions). Run it after changing `CLAUDE.md`, rules, skills, agents, plugins, MCP servers or settings; it uses part of the usage limit.

## Session profile

At the commit step the issue workflow runs `harness-kit:profile-session` and, after the user's yes, saves the session record under `.ai/benchmarks/sessions/records/`. `profile-session.config.json` maps the agents of this repository to activity categories.

## Porting to another project

1. Enable the plugins in the project's `.claude/settings.json`.
2. Run `/issue-kit:init-config` to draft `issue-kit.config.json` and the permission rules.
3. Write the project's own parts: `CLAUDE.md` and the specs it points to, the stack rules, the project agents and skills that the configuration names in `roles`, and the benchmark fixtures.

Preconditions: issues with an acceptance checklist and dependencies recorded as GitHub "blocked by" relations.

## Maintenance

When `CLAUDE.md`, `.claude/**`, `.ai/**`, `.mcp.json`, `issue-kit.config.json` or `profile-session.config.json` change, update this document in the same change, run `/issue-kit:init-config check`, and preferably the benchmark.
