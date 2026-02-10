# A1_5 — Unity ↔ App: boundary & transport contract (v0.1)

**Status:** ready for review  
**Owner:** Codex  
**Started:** 2026-02-10  
**Branch:** task/A1_5-unity-app-boundary-transport-contract

## Цель

Зафиксировать и реализовать минимальный контракт Unity ↔ app: команды через Named Pipe, кадры через MMF (ring‑buffer).

## Контекст / ссылки

- `docs/unity-app-boundary.md`
- `docs/app-api.md`

## Объём

**Включено:**
- Named Pipe `vtube_cmd_v1` (App → Unity, JSON SceneCommand)
- MMF `vtube_frames_v1` + сигнализация
- Минимальный прототип чтения кадра в app (snapshot/stream 503 → реальные)

**Не включено:**
- Оптимизации кодирования (JPEG/PNG)
- Дистанционная авторизация

## Шаги реализации

1. Определить структуру слота MMF (header + payload).
2. Реализовать writer в Unity (пишет последний кадр по formatId/profile).
3. Реализовать reader в app (берёт последний кадр).
4. Подключить к `/snapshot` и `/stream`.

## Критерии приёмки

- [ ] `/snapshot/<formatId>/<profile>.png` отдаёт картинку (на основе MMF).
- [ ] `/cmd` доставляет команды в Unity (через pipe).
- [ ] При отсутствии Unity app возвращает 503 без краша.

## Как проверить

- Запустить Unity + app, запросить `/snapshot`, увидеть валидный PNG.
- Отправить `/cmd` и увидеть лог в Unity (requestId).

## Артефакты

- Реализованный transport (Named Pipe + MMF)
- Обновлённая документация/схемы при необходимости

## Отчёт выполнения

### Что сделано
- Добавлен transport в app: Named Pipe сервер для `/cmd`, MMF reader + стор последнего кадра.
- `/snapshot` теперь отдаёт PNG из MMF, `/stream` — MJPEG поток.
- В Unity добавлены bootstrap + MMF writer (raw RGBA) и Named Pipe client с ack.
- Документация по MMF слоту уточнена (фиксированный header).

### Как проверить
1. Запустить Unity (сцену можно любую) — должен появиться объект `VTube.Transport` и логи о pipe.
2. Запустить app и запросить:
   - `GET /snapshot/cam_main_16x9/release.png` → валидный PNG.
   - `GET /stream/cam_main_16x9/release.mjpg` → MJPEG поток.
   - `POST /cmd` → Unity получает лог по requestId.

### Тесты
- `dotnet test app.Tests` — не выполнилось из-за отказа в доступе к NuGet cache (`/home/yakoo/.local/share/NuGet/http-cache`).

### Коммиты
- `b7fe4cd` feat(A1_5): unity-app transport mmf+pipe

### Риски / follow-up
- `System.Drawing` зависит от Windows-рантайма; если нужна кросс‑платформенность, потребуется замена энкодера.
