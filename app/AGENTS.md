# App — AGENTS.md

## Responsibilities
- Expose endpoints: /api/health, /api/streams, /view, /stream (MJPEG), /snapshot
- Manage consumers and on-demand activation
- Log/audit API actions and scene commands

## Defaults
- Bind localhost only by default.
- Token required for remote /view and /stream.

## Tests
- Unit tests for config parsing and consumer lifecycle.
