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
