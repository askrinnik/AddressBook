# Harness report: 2026-11-17-12-43_move-to-issue-kit-0-1-3-and-harness-kit

**Overall: worse in bench-base** (see Summary for what moved and why).

- **Note:** Move to issue-kit 0.1.3 and harness-kit plugins, drop Copilot, rules carry full standards
- **Setup:** commit b13f847e, uncommitted changes at start: yes; Claude Code 2.1.319, model Sonnet 5.5, effort medium, harness 3, MCP servers: none.
- **Fixtures** (one test each): bench-base, quality-vertical-slice.
- **Compared with:** the most recent earlier run, `2026-10-07-16-54_parity-with-githubbackup-shipping-order` (commit 65be4ffe); each fixture is compared with the previous run that contains it.

## How to read this report

Every fixture is one scripted Claude Code session, or several repeats of it. All token numbers come from the session transcript. The columns of the first table are explained below from left to right, one paragraph per column; the terms of the Quality table and the `~` mark follow.

**Fixture (Repeats).** The name of the test, and in brackets how many times its session was run. `bench-*` fixtures measure cost (what a session loads and what reading a file adds); `quality-*` fixtures also have a judge that scores the answer. Where the number of repeats is more than 1, the other numbers are averages, shown as "mean (min-max)": the average followed by the lowest and the highest repeat.

**Start ctx (`FirstCtx`).** Tokens in the very first model call, before any work: tool definitions, system prompt, CLAUDE.md, memory index, skill and agent lists. Every later call of the session re-reads it. It depends on the tools loaded, so compare it only between runs of the same fixture. The bracket `(Δ +25)` is the change against the previous run of this fixture; up to 50 tokens counts as unchanged.

**End ctx (`LastCtx`).** Tokens in the last model call: the start context plus everything the task added. The bracket is the change against the previous run.

**Added by task (`Read`).** End ctx minus Start ctx: the tokens the task put into the context, that is the file or files read, the rules loaded for them and the model's answer. This is the number that shows what the instructions cost per task. The bracket is the change against the previous run; for fixtures with repeats, a change is noise when it is at most the larger of the previous run's own range and 1000 tokens ("within the spread").

**Model calls.** How many requests the session sent to the model. Each call re-reads the whole context, so more calls cost more.

**Output tokens.** Tokens the model wrote (answers and tool calls).

**Cost units.** The price of the session in relative units, where one new input token costs 1. Output is dearer and cached input is cheaper, so the session's tokens are counted by kind and multiplied by a weight: `cost units = new input x 1 + cache read x 0.1 + cache write x 1.25 + output x 5`. *New input* is context that was not cached, *cache read* is context taken from the prompt cache (the whole context is re-read on every model call), *cache write* is context stored into the cache the first time, *output* is what the model wrote. The sums are over all model calls of the session. Use it to compare sessions by cost, not by size. The token counts and the weights of every session are stored in `run-history.csv` (`InputTok`, `CacheReadTok`, `CacheWriteTok`, `OutTok`, `WInput`, `WCacheRead`, `WCacheWrite`, `WOutput`), so the value can be recomputed from the row; the breakdown is under the context table. The weights are relative prices assumed by the skill, not amounts of money.

**Tools loaded.** The tools the session was given. A fixture that sets none gets every built-in tool, and their definitions (about 17000 tokens) are part of its start context; fixtures limited to `Read` or a few tools carry almost none, which is why their start context is much smaller.

**Rules loaded.** Instruction files pulled in when a file of a matching type is read, as name=tokens. A change here is a real change to the instructions. A "Rules vs previous run" column appears only when some fixture's rules changed.

**Quality table.** *Score* is the number of checklist items the judge found, with the lowest and highest repeat, out of *Maximum*. *False positives* are findings the judge rejected as not real problems, summed over the repeats (previous run in brackets). The *Verdict* is `within noise` (no real change), `REGRESSION` (the mean score fell by more than the larger of the previous run's range and 0.5, or there are two or more extra false positives), `improvement` (the same, upward) or `no previous run`. One extra false positive is only noted, because the judge varies by that much.

**`~` after a number.** MCP servers were not ready when the session started, so the number can be about 1000 tokens off (see Caveats).

## Context and cost per fixture

The value in brackets after a number, (Δ ...), is the change against the previous run (0 or a few tens of tokens is noise). Values are averages over the repeats; "mean (min-max)" is shown for fixtures that ran more than once. Start context depends on the tools loaded (see the Tools loaded column), so compare it only between runs of the same fixture, never between fixtures with different tools.

| Fixture<br>(Repeats) | Start ctx (`FirstCtx`) | End ctx (`LastCtx`) | Added by task (`Read`) | Model calls | Output tokens | Cost units | Tools loaded | Rules loaded | Rules vs previous run |
|---|---|---|---|---|---|---|---|---|---|
| bench-base (1) | 38911 (Δ +4026) | 38911 (Δ +4026) | 0 (Δ 0) | 1 | 6 | 29091 | all built-in<br>(15 tools, 18914 tokens) | none | unchanged |
| quality-vertical-slice (3) | 11921 (Δ +1494) | 23389 (Δ +977) | 11468 (10669-12523) (Δ -517) | 3.67 (3-4) | 3540 (3484-3598) | 40515 (36603-47714) | Read<br>Grep<br>Glob | api-architecture=741<br>csharp=466<br>docs=560<br>update-docs-on-code-change=474 | api-architecture 39->741, csharp 30->466, docs 462->560, -blazor (36) |

<details><summary>How the cost units were calculated</summary>

Tokens by kind (averages over the repeats) multiplied by their weight. Weights: new input 1, cache read 0.1, cache write 1.25, output 5 per token.

| Fixture | New input (x 1) | Cache read (x 0.1) | Cache write (x 1.25) | Output (x 5) | Cost units (recomputed) | Cost units (stored) |
|---|---|---|---|---|---|---|
| bench-base | 2 | 17024 | 21885 | 6 | 29091 | 29091 |
| quality-vertical-slice | 10 | 48467 | 14366 | 3540 | 40515 | 40515 |

Example, bench-base (first repeat): 2 x 1 + 17024 x 0.1 + 21885 x 1.25 + 6 x 5 = 29091.

</details>

## Summary

One line per fixture: what was measured, the comparison with the previous run, and the conclusion after the arrow.

- **bench-base**: **start context**: 38911 tokens in the first model call, before any work (tools, system prompt, CLAUDE.md, memory index, skill list); every later call re-reads it. Previous run: 34885, so +4026. Where it moved: built-in tool definitions +1294; system prompt +73; claude.md -653; skill list +1073; agent list +440; not attributable to any area (reminders, environment, git status) +1803; this run had uncommitted changes, whose file list is part of the start context and part of the unattributed difference (about 700 tokens per 30 files). **-> tokens worse (more tokens).**
- **quality-vertical-slice**: one session adds 11468 (10669-12523) tokens on average (range over 3 repeats) in 3.67 model calls on average; this covers the files the model chose to read, the rules loaded for them and its answer. Previous run: 11985 (10978-12553). Allowed difference: 1575 (the larger of that run's own range and 1000); actual difference: -517. Rules loaded changed: api-architecture 39->741, csharp 30->466, docs 462->560, -blazor (36). Judge: 3.83 of 4 checklist items found (lowest repeat 3.5) (previous run 4), 0 false positive(s). **-> tokens unchanged; quality unchanged (within noise).**

## Quality (judged fixtures)

Look at Verdict: `within noise` means the review quality did not change. Score is the average number of checklist items the judge found, with the lowest and highest repeat; false positives are summed over the repeats.

| Fixture | Score (mean, min-max) | Maximum | False positives (previous run) | Previous mean score | Δ | Verdict |
|---|---|---|---|---|---|---|
| quality-vertical-slice | 3.83 (3.5-4) | 4 | 0 (0) | 4 | -0.17 | within noise |

### Repeats that need a look

Repeats that scored below the maximum or raised a false positive; read the answer before repeating a judge verdict as fact.

- quality-vertical-slice repeat 3: 3.5/4, 0 false positive(s). Item 1 is partly met: it names Contact.cs, ContactConfiguration.cs, the migration, the contracts and the Web pages, but gives no RFC 7807 detail. Item 2 is partly met: both validators are covered with format and max length, but RFC 7807 problem details and the form display are not explicitly stated. Item 3 is met (create/update specs, boundary 254/255, invalid format, empty, null, factory and schema updates). Item 4 is met: it lists several decisions and asks for confirmation before implementing. No clear false positives; the Vue project and extra test projects are plausible for this repo.

<details><summary>Every repeat with the judge's notes and a link to the answer</summary>

| Fixture | Repeat | Score | False positives | Judge notes | Answer |
|---|---|---|---|---|---|
| quality-vertical-slice | 1 | 4/4 | 0 | Item 1: full slice named with real files, entity, config, migration, contracts and Create/Edit pages. Item 2: both validators, length and format, with the Blazor form annotations covered (RFC 7807 not explicitly mentioned, but acceptable). Item 3: Playwright tests in src/ApiTests with happy path, boundary, invalid format, empty value, and contact.factory.ts/schema. Item 4: nine questions raised and confirmation requested before implementing. No clear false positives. | [answer](../runs/2026-11-17-12-43_move-to-issue-kit-0-1-3-and-harness-kit_quality-vertical-slice-1.md) |
| quality-vertical-slice | 2 | 4/4 | 0 | Item 1: names the entity, configuration, migration, contracts and Create/Edit Razor pages. Item 2: both validators with EmailAddress and max length matching the column. It does not explicitly mention RFC 7807 problem details, but the form-error handling is covered, so I count it as satisfied. Item 3: Playwright API tests cover happy path, 254/255 boundary, invalid format, null and empty, and reuse the factory and schema. Item 4: seven decisions raised for confirmation before implementation. No clear false positives. | [answer](../runs/2026-11-17-12-43_move-to-issue-kit-0-1-3-and-harness-kit_quality-vertical-slice-2.md) |
| quality-vertical-slice | 3 | 3.5/4 | 0 | Item 1 is partly met: it names Contact.cs, ContactConfiguration.cs, the migration, the contracts and the Web pages, but gives no RFC 7807 detail. Item 2 is partly met: both validators are covered with format and max length, but RFC 7807 problem details and the form display are not explicitly stated. Item 3 is met (create/update specs, boundary 254/255, invalid format, empty, null, factory and schema updates). Item 4 is met: it lists several decisions and asks for confirmation before implementing. No clear false positives; the Vue project and extra test projects are plausible for this repo. | [answer](../runs/2026-11-17-12-43_move-to-issue-kit-0-1-3-and-harness-kit_quality-vertical-slice-3.md) |

</details>

## Instruction files that differ from the most recent earlier run

Changes in rules, agents, skills, `CLAUDE.md` and settings from `65be4ffe` to the working tree at report time (this run had uncommitted changes) :

```
 .ai/prompts/implement-issue.md                     | 190 ---------
 .ai/prompts/implement-issues.md                    |  77 ----
 .ai/prompts/next-issue.md                          |   5 -
 .claude/agents/architect.md                        |   4 +-
 .claude/agents/build-runner.md                     |  61 ---
 .claude/agents/csharp-dotnet-janitor.md            |   6 +-
 .claude/agents/issue-developer.md                  |  44 ---
 .claude/agents/issue-planner.md                    |  46 ---
 .claude/agents/issue-verifier.md                   |   4 +-
 .claude/agents/orchestrator.md                     |   4 +-
 .claude/agents/playwright-tester.md                |   6 +-
 .claude/agents/project-documenter.md               |   2 +-
 .claude/agents/security-reviewer.md                |   4 +-
 .claude/agents/skill-runner.md                     |  41 --
 .claude/agents/software-engineer.md                |   6 +-
 .claude/commands/implement-issue.md                |   9 -
 .claude/commands/implement-issues.md               |   9 -
 .claude/commands/next-issue.md                     |   9 -
 .claude/rules/api-architecture.md                  |  58 ++-
 .claude/rules/blazor.md                            |  44 ++-
 .claude/rules/bunit.md                             |  96 ++++-
 .claude/rules/csharp.md                            |  35 +-
 .claude/rules/docs.md                              |  11 +-
 .claude/rules/github-actions.md                    |   3 +-
 .claude/rules/playwright.md                        | 100 ++++-
 .claude/rules/update-docs-on-code-change.md        |   8 +-
 .claude/rules/vue.md                               |  61 +++
 .claude/settings.json                              |  50 ++-
 .claude/skills/_local.debug-issue/SKILL.md         |   6 +-
 .claude/skills/_local.git-commit/SKILL.md          |  83 ----
 .claude/skills/_local.github-issue/SKILL.md        |  95 -----
 .../scripts/Set-AcceptanceChecks.ps1               | 132 -------
 .../skills/_local.harness-quality-check/SKILL.md   | 120 ------
 .../scripts/Collect-Harness.ps1                    |  93 -----
 .../scripts/Run-Harness.ps1                        | 156 --------
 .../scripts/lib/Judge.ps1                          |  45 ---
 .../scripts/lib/Read-Session.ps1                   | 138 -------
 .../scripts/lib/Report.ps1                         | 428 ---------------------
 .claude/skills/_local.next-issue/SKILL.md          |  41 --
 .../_local.next-issue/scripts/Get-NextIssue.ps1    | 159 --------
 .../skills/_local.nuget-package-update/SKILL.md    |   8 +-
 .../scripts/Prepare-PackageUpdate.ps1              |   4 +-
 .claude/skills/_local.open-pr/SKILL.md             |  51 ---
 .claude/skills/_local.refactor-code/SKILL.md       |   6 +-
 .claude/skills/_local.run-api/SKILL.md             |   2 -
 .claude/skills/_local.run-tests/SKILL.md           |   2 -
 .../skills/_local.sync-ai-customizations/SKILL.md  |  59 ---
 .../scripts/check.ps1                              | 280 --------------
 .claude/skills/_local.verify-feature/SKILL.md      |   4 +-
 .claude/skills/_local.write-tests/SKILL.md         |  10 +-
 .claude/skills/aspnet-core/SKILL.md                |   2 -
 .claude/skills/code-review-checklist/SKILL.md      |   9 +-
 .claude/skills/document-workflow/SKILL.md          |   9 +-
 .claude/skills/update-docs/SKILL.md                |   4 +-
 CLAUDE.md                                          |  79 ++--
 55 files changed, 509 insertions(+), 2509 deletions(-)
```

## Start context by area (bench-base, client claude-desktop)

What the first model call consists of, so a change in start context can be traced to an area. Token sizes are estimates; the last row is the exact figure reported by the API.

<details><summary>Area table, MCP servers, skills and instruction files</summary>

| Area | Tokens (est.) | Share of start context | Δ vs previous run |
|---|---|---|---|
| Built-in tool definitions | 18914 | 49 % | +1294 |
| MCP tool definitions (loaded) | 0 | 0 % | 0 |
| System prompt | 1792 | 5 % | +73 |
| CLAUDE.md | 2366 | 6 % | -653 |
| Memory index | 216 | 1 % | -5 |
| Skill list | 7505 | 19 % | +1073 |
| Agent list | 2459 | 6 % | +440 |
| Deferred tool names | 48 | 0 % | +1 |
| MCP server instructions | 0 | 0 % | 0 |
| Not accounted for (reminders, environment, git status, estimate error) | 5611 | 14 % | |
| **FirstCtx (API)** | **38911** | 100% | +4026 |

Largest tool definitions: PowerShell=4541; Workflow=2344; Agent=2280; ScheduleWakeup=2099; Bash=1681; Grep=1010

### MCP servers

None: the run started its sessions without MCP servers, so this start context is what the repository itself loads.

### Skills, agents and instruction files

- Skills listed: 61, 7505 tokens. By source (count/tokens): local=7/658; other=41/4076; plugin=13/2527. Largest: dataviz=385; harness-kit=342; anthropic-skills:docs=274; anthropic-skills:pptx=262; anthropic-skills:google-workspace=260.
- Agent types listed: 19, 2459 tokens.
- Instruction files loaded at start (tokens, est.): CLAUDE.md=796; CLAUDE.md=2366; MEMORY.md=216 (CLAUDE.md 73 lines, memory index 8 lines).

</details>

## History

Start context, end context, added tokens and score of each fixture over the latest 8 runs that contain it; use it to see whether a number is stable over time.

<details><summary>History tables per fixture</summary>

### bench-base

| Run | Commit | Uncommitted | Harness | Start ctx | End ctx | Added by task | Score |
|---|---|---|---|---|---|---|---|
| 2026-10-05-11-15_old-harness-on-main | 58259490 | yes | 3 | 28684 | 28684 | 0 |  |
| 2026-10-05-11-19_new-harness-after-184 | 63d85a8c | no | 3 | 36194 | 36194 | 0 |  |
| 2026-10-05-11-35_rules-without-imports-fewer-skills | a1212beb | no | 3 | 30696 | 30696 | 0 |  |
| 2026-10-07-16-54_parity-with-githubbackup-shipping-order | 65be4ffe | no | 3 | 34885 | 34885 | 0 |  |
| 2026-11-17-12-43_move-to-issue-kit-0-1-3-and-harness-kit | b13f847e | yes | 3 | 38911 | 38911 | 0 |  |

### quality-vertical-slice

| Run | Commit | Uncommitted | Harness | Start ctx | End ctx | Added by task | Score |
|---|---|---|---|---|---|---|---|
| 2026-10-05-11-15_old-harness-on-main | 58259490 | yes | 3 | 8212 | 24899 | 16687 | 3.67/4 |
| 2026-10-05-11-19_new-harness-after-184 | 63d85a8c | no | 3 | 13212 | 26093 | 12881 | 3.33/4 |
| 2026-10-05-11-35_rules-without-imports-fewer-skills | a1212beb | no | 3 | 8824 | 21838 | 13014 | 4/4 |
| 2026-10-07-16-54_parity-with-githubbackup-shipping-order | 65be4ffe | no | 3 | 10427 | 22412 | 11985 | 4/4 |
| 2026-11-17-12-43_move-to-issue-kit-0-1-3-and-harness-kit | b13f847e | yes | 3 | 11921 | 23389 | 11468 | 3.83/4 |

</details>

## Caveats

- The fixture changed since the previous run, so the comparison is indicative only. Applies to: quality-vertical-slice.
- The run had uncommitted changes: their file list is part of the start context and the commit id alone does not identify the instructions that were tested. Applies to: all fixtures.

