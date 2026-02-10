# D2_3 — Output profiles: binding config → stream/snapshot settings

**Status:** todo

## Цель

Связать `outputs/outputs.json` с настройками стрима/снапшота (fps, jpegQuality, includeTags).

## Контекст / ссылки

- `docs/vtube-mvp.md` (Output Profiles)

## Объём

**Включено:**

- Загрузка output profiles
- Привязка профиля к formatId (release/debug)
- Применение fps/jpegQuality/idleTimeout

**Не включено:**

- UI для редактирования профилей

## Шаги реализации

1. Загрузить `outputs/outputs.json` и модель профилей.
2. Связать formatId → outputProfileId (из formats.json).
3. Применить настройки в `/stream` и `/snapshot`.

## Критерии приёмки

- [ ] Разные профили дают разный fps/jpegQuality.
- [ ] Если профиль не найден — понятная ошибка.

## Как проверить

- Задать release/debug разные значения и сравнить.

## Артефакты

- OutputProfile model
- Config loader binding
