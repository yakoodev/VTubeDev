# vtproj (MVP)

## Минимальный состав

`vtproj` — git‑friendly папка проекта. В MVP минимальный состав такой:

```text
<project>/
  vtproj/
    scene/
      scene.json
    outputs/
      outputs.json
    ui/
      overlays/
        <overlayProfileId>/
          overlay.json
          assets/
```

`workspace.json` — локальный профиль пользователя, хранится вне `vtproj`.
`.env` — секреты, тоже вне `vtproj`.

## Версии форматов

Во всех ключевых JSON есть `*Version`:

- `scene/scene.json`: `sceneVersion`
- `outputs/outputs.json`: `outputsVersion`
- `ui/overlays/<overlayProfileId>/overlay.json`: `overlayFormatVersion`
- `workspace.json`: `workspaceVersion`

Нумерация MVP: `"1.0"`.

## Примеры минимальных файлов

### `scene/scene.json`

```json
{
  "sceneVersion": "1.0",
  "id": "scene_main",
  "name": "Main Scene",
  "defaultCameraId": "cam_main",
  "cameras": {
    "cam_main": {
      "name": "Main Camera"
    }
  }
}
```

### `outputs/outputs.json`

```json
{
  "outputsVersion": "1.0",
  "profiles": {
    "Release30": { "fps": 30, "jpegQuality": 80, "idleTimeoutSec": 10, "includeTags": [] },
    "Debug15":   { "fps": 15, "jpegQuality": 60, "idleTimeoutSec": 5,  "includeTags": ["debug"] }
  }
}
```

### `ui/overlays/<overlayProfileId>/overlay.json`

```json
{
  "overlayFormatVersion": "1.0",
  "id": "overlay_default",
  "design": { "width": 1920, "height": 1080 },
  "scaling": { "mode": "fit", "snapToPixels": true },
  "safeArea": { "default": { "left": 0, "top": 0, "right": 0, "bottom": 0 } },
  "widgets": [],
  "tags": []
}
```

### `workspace.json` (локальный профиль)

```json
{
  "workspaceVersion": "1.0",
  "lastProjectPath": "<path-to-project>",
  "lastSceneId": "scene_main",
  "lastOverlayProfileId": "overlay_default"
}
```

## Важно

- Производные форматы не должны вызывать дополнительный 3D‑рендер.
- `vtproj` — единственный источник правды для shared/версионируемого состояния.
- Локальные состояния, кеши и секреты — вне `vtproj`.
