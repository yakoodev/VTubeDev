# Unity — AGENTS.md

## Safety
- Don’t touch ProjectSettings unless explicitly requested.
- Don’t delete/regenerate .meta files.
- Avoid mass prefab rewrites.

## Rendering
- Render pipeline changes only via URP RendererFeature/Pass.
- Avoid per-frame allocations in hot paths.

## Validation
- Keep Sandbox scene runnable.
