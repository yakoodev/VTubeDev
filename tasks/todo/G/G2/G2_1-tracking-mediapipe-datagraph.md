# G2_1 — Tracking: MediaPipe + DataGraph базовый поток

**Status:** todo

## Цель

Подключить MediaPipe трекинг и базовый DataGraph поток до DataStore.

## Контекст / ссылки

- `docs/vtube-mvp.md` (Tracking)

## Объём

**Включено:**

- MediaPipe источник
- Базовый DataGraph pipeline
- Запись параметров в DataStore

**Не включено:**

- Сложные фильтры и калибровки

## Шаги реализации

1. Интегрировать MediaPipe.
2. Создать граф: source → filters → ParameterSet.
3. Записывать параметры в DataStore.

## Критерии приёмки

- [ ] Трекинг обновляет параметры в DataStore.

## Как проверить

- Запустить и увидеть изменения параметров в debug UI/логах (допустимы заглушки, если MediaPipe ещё не интегрирован).

## Артефакты

- Tracking module
- DataGraph scaffolding
