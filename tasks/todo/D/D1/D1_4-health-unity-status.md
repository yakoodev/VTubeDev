# D1_4 — /api/health: статус Unity boundary

**Status:** todo

## Цель

Добавить в /api/health сведения о подключении Unity (pipe/MMF readiness).

## Контекст / ссылки

- `docs/unity-app-boundary.md`
- `docs/vtube-mvp.md`

## Объём

**Включено:**

- Поля `unityConnected`, `framesAvailable` или аналог
- 503 при полной недоступности (опционально)

**Не включено:**

- Детальная диагностика процессов Unity

## Шаги реализации

1. Определить DTO health с полями Unity состояния.
2. Привязать к transport layer (pipe/MMF).
3. Обновить /api/health ответ.

## Критерии приёмки

- [ ] /api/health показывает состояние Unity.
- [ ] При отсутствии Unity статус корректный.

## Как проверить

- Запустить app без Unity: статус показывает disconnected.
- Запустить Unity: статус показывает connected.

## Артефакты

- Обновлённый /api/health
