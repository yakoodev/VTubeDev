# Agent Autonomy Notes

This document captures the additional rules needed for autonomous task execution.

## Task Selection
- Use `tasks/TASKS_INDEX.md` as the source of truth for available tasks.
- Apply the selection rule from that file to pick the next task.

## Branching and PRs
- Create a branch per task: `task/<TASK_ID>-<short-slug>`.
- If authenticated push is available, push the branch to the `yakoodev/VTubeDev` remote.
- PR creation requires a GitHub workflow or CLI. If `gh` CLI is not installed or not configured, stop and ask for guidance before creating the PR.

## Tests and Checks
- Always read the task file for required checks.
- If the task lists checks, run them.
- If the task does not list checks, no tests are required by default.

## Unity MCP
- When a Unity MCP is available, use it for:
  - Opening the Unity project
  - Running the project
  - Viewing logs
  - Capturing and analyzing screenshots
  - Running Unity tests
- If a specific MCP command is required and not documented, ask for the command list.

## Safety
- Never print secrets to logs.
- Do not read or expose `.env` contents.
