# Agent Pipeline

This guide makes the "выполни задачу" loop explicit so the agent can act on it without waiting for extra prompts.

## 1. Trigger
- The user says `выполни задачу`. That is the only signal the agent needs to start working. No other files should be modified until the agent has chosen and claimed a task.
- If there are blocking issues (merge conflicts, missing tools) report them before claiming anything.

## 2. Claim the next task
- Before starting any task:
  - `git checkout main`
  - `git pull`
  - `git status -sb`
- Scan `tasks/todo/` using the selection rule in `tasks/TASKS_INDEX.md` (stage A..I then X, substage numeric order, lowest task index). Do not hard-code names.
- Once the target task is identified, move it from `tasks/todo/...` to the identical path under `tasks/inProgress/...`.
- Add or update the metadata block at the top of the task file:
  - `Status: inProgress`
  - `Owner: Codex`
  - `Started: YYYY-MM-DD`
  - `Branch: task/<TASK_ID>-<short-slug>` (the branch you will create next)

## 3. Start work and branch
- Create the Git branch named after the task: `task/<TASK_ID>-<short-slug>` (slug should be ASCII and descriptive).
- Unity-generated files (Library/Logs/*.csproj/*.sln/etc.) are expected and should not block work. Do not add them to commits.
- Keep the scope tight: implement only what the task describes. If new work is required, note it in the task as a follow-up task, but do not expand the current PR.
- Run lint/build/tests listed in the task or in the relevant docs (see `docs/agent-hints.md` for what can be run automatically, especially Unity smoke/tests).

## 4. Commits and PR
- Commit frequently with clear messages using the existing convention (`feat`, `fix`, `docs`, etc.).
- When the work satisfies the task, create a pull request (GitHub) with:
  - a short summary of what changed;
  - how to verify;
  - any manual work still needed.
- Push the branch before opening the PR. Use the `gh` CLI if available, otherwise document the PR creation steps in the task report.

## 5. Reporting
- Append a completion report inside the task under an `## Отчёт выполнения` section containing:
  - What happened (summary of work).
  - How to verify (commands or manual steps).
  - Commits made (hashes or descriptions).
  - Outstanding risks or follow-ups.
- Do not move the task to `completed/`; the user will move it manually after manual verification. You can mark `Status: ready for review` if needed.

## 6. Aftermath
- If the PR lands cleanly and no follow-up tasks are open, wait for the next `выполни задачу` command.
- If the user needs more changes, keep details in the task file (follow-up, blockers, etc.).
- Once the user moves the task into `tasks/completed/`, stop touching it, even if the branch still exists.

This pipeline keeps the agent in sync with the Kanban flow and lets the user stay in control when they want to gate the move to `completed/`.
