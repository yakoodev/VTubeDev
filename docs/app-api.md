# App API (draft)

This document captures the initial HTTP API surface for the local app/server. It is a draft meant to align Unity and app work before implementation.

## Endpoints

### `GET /api/health`
Returns basic status for diagnostics.

Response (200):
```json
{
  "status": "ok",
  "build": "dev",
  "timeUtc": "2026-02-10T00:00:00Z"
}
```

### `GET /api/streams`
Lists available streams (format/profile) and URLs.

Response (200):
```json
{
  "streams": [
    {
      "formatId": "cam_main_16x9",
      "profile": "release",
      "viewUrl": "/view/cam_main_16x9/release",
      "streamUrl": "/stream/cam_main_16x9/release.mjpg",
      "snapshotUrl": "/snapshot/cam_main_16x9/release.png"
    }
  ]
}
```

### `GET /view/<formatId>/<profile>`
Simple HTML viewer for OBS Browser Source. Must render the MJPEG stream or fall back to snapshot polling.

### `GET /stream/<formatId>/<profile>.mjpg`
MJPEG stream.

### `GET /snapshot/<formatId>/<profile>.png`
Latest PNG frame. Return 503 if no frame is available.

### `POST /cmd`
Submit a `SceneCommand` to the command queue.

Request:
```json
{
  "requestId": "req_123",
  "source": "web",
  "timestampUtc": "2026-02-10T00:00:00Z",
  "command": {
    "type": "SetInputSource",
    "payload": { "mode": "VideoSim" }
  }
}
```

Response (202):
```json
{
  "status": "accepted",
  "requestId": "req_123"
}
```

Error (400):
```json
{
  "status": "rejected",
  "requestId": "req_123",
  "error": "Validation failed: command.type is required"
}
```

## SceneCommand DTO (draft)

```json
{
  "type": "SetInputSource",
  "payload": { "mode": "VideoSim" }
}
```

Notes:
- `requestId` should be echoed in logs and response.
- `source` should be one of: `ui`, `web`, `hotkey`.
- Validation failures must return 400 and a readable error.
