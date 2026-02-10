# A1_5 — Unity ↔ App: boundary & transport contract (v0.1)

**Status:** todo

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
