# F3_1 — EventEnvelope + Normalizer

**Status:** todo

## Цель

Ввести единый формат входящих событий и нормализацию до EventEnvelope.

## Контекст / ссылки

- `docs/vtube-mvp.md` (Integrations/Rule Engine)

## Объём

**Включено:**

- EventEnvelope DTO
- Normalizer для входящих событий

**Не включено:**

- Реальные интеграции источников

## Шаги реализации

1. Определить EventEnvelope схему.
2. Реализовать Normalizer интерфейс.

## Критерии приёмки

- [ ] Любое сырьё приводится к EventEnvelope.

## Как проверить

- Смоук: создать fake event и нормализовать.

## Артефакты

- EventEnvelope DTO
- Normalizer
