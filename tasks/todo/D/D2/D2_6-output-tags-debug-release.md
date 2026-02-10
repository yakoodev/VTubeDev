# D2_6 — Output includeTags (debug/release)

**Status:** todo

## Цель

Применить includeTags из output profile для debug/release overlay‑виджетов.

## Контекст / ссылки

- `docs/vtube-mvp.md` (Output profiles, includeTags)
- Зависит от D2_3

## Объём

**Включено:**

- includeTags фильтрация
- Debug теги не попадают в release

**Не включено:**

- Сложные правила включения

## Шаги реализации

1. При рендере overlay учитывать includeTags профиля.
2. Скрывать виджеты, чьи теги не входят в includeTags.

## Критерии приёмки

- [ ] Debug‑виджеты отображаются только в debug профиле.
- [ ] Release профиль исключает debug‑теги.

## Как проверить

- Иметь overlay с tag `debug`, сравнить release/debug.

## Артефакты

- Tag filtering
