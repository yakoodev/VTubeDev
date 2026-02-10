# VTube — AGENTS.md (repo root)

## Project summary
Unity (URP) renders base frames per cameraId and composes per formatId (crop/scale + overlay).
Derived formats must NOT trigger extra 3D renders.

## Repo layout
- unity/: Unity project
- app/: local web server + orchestrator
- shared/: shared DTO/contracts (optional)
- tools/: scripts
- examples/: sample configs
- docs/: notes

## Rules
- Do not commit secrets (.env).
- Keep Unity .meta files intact (no GUID regeneration).
- Prefer small, reviewable changes.
- Before big edits: plan (bullets) + list files to change.
- For Unity testing/validation, use MCP tools by default; if MCP is unavailable, state why and what was done instead.
- When the user says `выполни задачу` in repo root, first switch to `main` and pull latest updates, then follow `docs/agent-pipeline.md` and `tasks/AGENTS.md` to the letter (pick the next task via `tasks/TASKS_INDEX.md`, move it to `tasks/inProgress/`, create a branch, commit, test, push, open PR).
- When the user says the task is done (e.g., “задача выполнена”), switch to `main`, pull latest updates, and move the task you worked on to `tasks/completed/` (this happens after PR verification).
