# A1_1 — vtproj: минимальный состав и *Version для MVP

**Status:** completed  
**Finished:** 2026-02-10


## Цель

Зафиксировать минимальный состав vtproj и добавить *Version в ключевые JSON, чтобы дальнейшие фичи не превращали конфиги в "битый фарш".


## Контекст / ссылки

В архитектурном холсте vtproj — git-friendly папка с конфигами (scene/ui/outputs/integrations). Секреты в .env, локальное состояние в workspace.json.


## Объём

**Включено:**

- Список файлов/папок MVP
- *Version для scene/outputs/overlays/workspace
- README/дока по структуре


**Не включено:**

- Миграции между версиями (отдельная задача)




## Шаги реализации

1. Описать минимальные файлы: `vtproj/scene/scene.json`, `vtproj/outputs/outputs.json`, `vtproj/ui/overlays/*.json`. `workspace.json` — локальный файл профиля пользователя (НЕ внутри vtproj).
2. Добавить `*Version` (например, `outputsVersion`, `formatsVersion`, `overlayFormatVersion`, `workspaceVersion`) и договориться о нумерации (например, 1.0).
3. Добавить док `docs/vtproj.md` с примерами минимальных файлов.


## Критерии приёмки

- [x] В документации есть чёткий список файлов vtproj для MVP.
- [x] Во всех JSON MVP есть соответствующий *Version.
- [x] Примеры JSON валидны и запускаются.


## Как проверить

- Создать пустой vtproj по доке, запустить проект — старт без ошибок.


## Артефакты

- docs/vtproj.md
- Примеры JSON в vtproj/examples (или аналог)


## Отчёт выполнения
- Что сделано:
  - Описан минимальный состав `vtproj` и версионирование в `docs/vtproj.md`.
  - Добавлен минимальный пример проекта в `examples/vtproj-minimal`.
- Как проверить:
  - Проверить JSON на валидность, например `python3 -m json.tool <file>`.
- Коммиты:
  - `docs(A1_1): vtproj minimal structure + examples`
- Риски/заметки:
  - Авто‑проверки отсутствуют, опираемся на ручную проверку.
