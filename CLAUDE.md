# CLAUDE.md — AddressBook

Project guidance for **Claude Code**.

This file is intentionally a **thin hub**: it owns only the few rules that live nowhere else, and points to the authoritative docs for everything else. Nothing here restates build commands, project structure, or coding conventions — those have a single source of truth (the specs and the rules in `.claude/rules/`), and duplicating them here would only drift out of sync.

## Always

- **All comments in source code (`//`, `///` XML docs, `<!-- -->`, `#`) MUST be written in English** — never Russian or any other language. If you touch a file that still has non-English comments, translate them. (Docs under `docs/` and READMEs are not source code and are unaffected.)
- **A comment states what the code does and why, in the present tense, and makes sense without the issue tracker or the file's history.** Never write change narration ("added", "now", "previously", "fixed"), issue or task references (`#123`, `B15`), line numbers, or a restatement of what the code plainly says.
- When a Git commit is requested, use the `issue-kit:git-commit` skill and follow its message conventions; do not hand-write commit messages in another format. Pull requests follow `issue-kit:open-pr`, issue comments `issue-kit:github-issue`. The plugin's `PostToolUse` hook checks every new commit message.
- **Never `git commit` or `git push` on your own initiative.** Do it only when the user's current message explicitly asks for it (for example "commit", "push", "open a PR"). Permission covers only the action named and does not carry over to later edits: after the user asks for further changes — even on an already-pushed branch or open PR — make the changes, verify them, then stop and ask before committing or pushing again. Never force-push or rewrite pushed history without an explicit request.
- **Never comment on, create or edit an issue, and never open or merge a pull request, without the user's explicit go-ahead for that action.** The one standing exception: starting `/issue-kit:implement-issues` authorises its commits (one per issue); push, PR and comments still wait for a go-ahead unless the command carries `--ship`. A PR targets `main`, its description contains `Closes #<n>`, and the user merges it with a merge commit.
- **Never create, switch, rename or delete a branch, and never stash, on your own initiative** — not to prepare work, not to tidy up after a mistake. Do it only when the user's current message asks for it. The one built-in exception is a task branch created together with a commit the user has asked for; the `issue-kit:implement-issue` and `issue-kit:implement-issues` skills say when. Work stays on the branch you were started on; if that is a problem, say so and ask.

## Where things live (read the source — do not restate it here)

- **Architecture, project structure, CI, deployment** → [docs/specs/Architecture.md](docs/specs/Architecture.md)
- **API — build/run, endpoints, CQRS (MediatR) / FluentValidation / RFC 7807, domain, data access** → [docs/specs/AddressBook.Api.md](docs/specs/AddressBook.Api.md)
- **Web — build/run, launch profiles & ports, Blazor WASM + MudBlazor conventions, API service, pages** → [docs/specs/AddressBook.Web.md](docs/specs/AddressBook.Web.md)
- **Vue frontend — build/run, configuration, Vuetify components, API client, pages, deliberate differences from Blazor** → [docs/specs/AddressBook.Vue.md](docs/specs/AddressBook.Vue.md); how to run it: [src/AddressBook.Vue/README.md](src/AddressBook.Vue/README.md)
- **Vue component tests (Vitest) — run & conventions** → [src/AddressBook.Vue/README.md](src/AddressBook.Vue/README.md#tests), [.claude/rules/vue.md](.claude/rules/vue.md)
- **Learning guide: Vue for Blazor developers (in Russian)** → [docs/guides/vue-for-blazor-developers.md](docs/guides/vue-for-blazor-developers.md)
- **Contracts — commands, queries, models** → [docs/specs/AddressBook.Contracts.md](docs/specs/AddressBook.Contracts.md)
- **API E2E tests — run & conventions** → [src/ApiTests/README.md](src/ApiTests/README.md)
- **Web component tests (bUnit) — run & conventions** → [src/AddressBook.Web.Tests/README.md](src/AddressBook.Web.Tests/README.md), [.claude/rules/bunit.md](.claude/rules/bunit.md)
- **UI E2E tests (planned)** → [docs/tasks/ui-tests-framework-plan.md](docs/tasks/ui-tests-framework-plan.md)

Solution file: `src/AddressBook.slnx`.

## Working style

- Make focused, reviewable changes; reuse existing abstractions before adding new ones.
- **Run simple commands.** The working directory is already the repository root: do not prefix commands with `cd`, and avoid `&&` chains, pipes, loops and heredocs where one plain command does the job. Plain commands match the permission rules in `.claude/settings.json`; compound ones cannot be allowed permanently and interrupt the user with a one-time prompt. When text must reach a command (commit message, comment, PR body), write it to a file in the scratchpad and pass the file (`-F`, `--body-file`).
- When a change spans the API and the Web frontend, wire both sides so the feature is complete.
- For a feature request, list the decisions neither the request nor the code settles (lengths and formats, required vs optional, uniqueness, where it shows and whether it is searchable, existing data) and ask the user to confirm them before implementing — do not pick them silently. Routine naming and layout calls are yours.
- New or changed API behaviour ships with Playwright tests in `src/ApiTests` (happy path, boundaries, negatives).
- Entry points: `/issue-kit:implement-issue <n>` takes one issue (bug, feature or tests) end to end; `/issue-kit:implement-issues <n> <n> …` takes several small issues in order on one branch, one commit each, and ships one PR; `/issue-kit:next-issue` recommends what to take next. The workflow comes from the `issue-kit` plugin of the `askrinnik/ai-plugins` marketplace; `issue-kit.config.json` holds this repository's values (commands, plan location, lanes, project agents in `roles`). How the AI harness is built: [docs/ai/README.md](docs/ai/README.md).

## File-type coding standards

The rules in `.claude/rules/` are the single source of truth for these standards. Claude Code loads a rule when it works with a file that matches the rule's `paths:`. If a standard did not load, read the rule from this table before editing.

| Topic / file type | Rule |
|---|---|
| API architecture (`src/AddressBook.Api/**`) | `.claude/rules/api-architecture.md` |
| C# (`**/*.cs`) | `.claude/rules/csharp.md` |
| Blazor project-specific (MudBlazor / WASM) | `.claude/rules/blazor.md` |
| Playwright E2E — API (`src/ApiTests/**`) and UI (`src/UiTests/**`) | `.claude/rules/playwright.md` |
| bUnit component tests (`src/AddressBook.Web.Tests/**`) | `.claude/rules/bunit.md` |
| Vue frontend and its Vitest tests (`src/AddressBook.Vue/**`) | `.claude/rules/vue.md` |
| GitHub Actions workflows (`.github/workflows/**`) | `.claude/rules/github-actions.md` |

> The Playwright rule covers both the API suite and the UI E2E suite (`src/UiTests`, in its own section). Claude Code also loads `src/UiTests/CLAUDE.md` and `src/AddressBook.Web.Tests/CLAUDE.md` when working in those subtrees.

The generic checklists — `security-owasp`, `web-performance`, `code-review-checklist`, `update-docs` — are **on-demand skills**; invoke them (or the built-in `/code-review` and `/security-review`) when that pass is actually needed.

`.claude/rules/update-docs-on-code-change.md` says which document each kind of code change updates, in the same change.

## Custom agents (`.claude/agents/`)

The workflow agents `issue-kit:issue-planner`, `issue-kit:issue-developer`, `issue-kit:build-runner` and `issue-kit:skill-runner` come from the plugin; `issue-kit.config.json` names the project agents below in its `roles`.

- **issue-verifier** (Sonnet, `roles.verifier`) — reproduces a bug or walks the acceptance items in a real browser; returns an evidence table.
- **playwright-tester** (Sonnet, `roles.testAuthor`) — writes and stabilises the Playwright API/UI E2E tests.
- **security-reviewer** (Opus, `roles.securityReviewer`) — read-only security review of a change, run when API, configuration or packages change.
- **architect** (Opus, `roles.architect`) — designs changes that preserve project boundaries; surfaces trade-offs.
- **software-engineer** — implements features/fixes across the API + Blazor stack outside the issue workflow.
- **csharp-dotnet-janitor** — C#/.NET cleanup, modernization, tech-debt remediation.
- **project-documenter** — generates project documentation (read-only on source).
- **orchestrator** — decomposes a multi-step request and delegates to the specialists.

Roles a Claude Code built-in already covers have no project agent: planning → the `Plan` agent; research → `Explore` / general-purpose; review → the `/code-review` skill.
