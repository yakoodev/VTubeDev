# F3_3 — Интеграции: Webhook + Hotkey + Twitch (MVP)

**Status:** todo

## Цель

Подключить 3 источника событий для MVP: webhook, hotkey, twitch.

## Контекст / ссылки

- `docs/vtube-mvp.md` (Integrations)
- Зависит от F3_1 и F3_2

## Объём

**Включено:**

- Webhook endpoint
- Hotkey listener
- Twitch chat ingest

**Не включено:**

- UI управления интеграциями

## Шаги реализации

1. Webhook: принимать событие → EventEnvelope.
2. Hotkey: локальные события → EventEnvelope.
3. Twitch: чат сообщения → EventEnvelope.

## Критерии приёмки

- [ ] Каждая интеграция вызывает события.
- [ ] События доходят до Rule Engine.

## Как проверить

- Отправить webhook и увидеть SceneCommand в логах.
- Для Twitch/Hotkey допускается заглушка/тестовый источник при отсутствии реальных ключей.

## Артефакты

- Интеграции
- Документация по настройке
