# I1_1 — Render regression: capture + golden + diff report

**Status:** todo


## Цель

Сделать контур регрессий: снятие snapshot по профилям, сравнение с golden и генерация diff-артефактов.


## Контекст / зависимости

- Требует рабочий /snapshot (D2_1) и базовый output pipeline.

## Объём

**Включено:**

- Capture tool
- Golden storage
- Pixel diff + threshold
- Diff image


**Не включено:**

- Перцептивные метрики




## Шаги реализации

1. Определить директории `tests/golden` и `tests/diff`.
2. Сделать утилиту (Editor menu или CLI) 'Capture snapshots for all profiles'.
3. Сделать сравнение PNG с порогом и генерацией diff.png.
4. Добавить доку 'как обновлять golden'.


## Критерии приёмки

- [ ] Можно сгенерировать golden одним действием.
- [ ] При изменении overlay diff падает и показывает diff.png.


## Как проверить

- Сгенерировать golden, поменять текст в overlay → сравнение падает.
- Вернуть обратно → проходит.


## Артефакты

- Capture tool
- Compare tool
- Docs
