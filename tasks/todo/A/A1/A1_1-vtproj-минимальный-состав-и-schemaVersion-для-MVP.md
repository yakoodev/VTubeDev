# A1_1 — vtproj: минимальный состав и *Version для MVP

**Status:** todo


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

1. Описать минимальные файлы: `vtproj/scene/scene.json`, `vtproj/outputs/outputs.json`, `vtproj/ui/overlays/*.json`, `vtproj/workspace.json` (локально).
2. Добавить `*Version` (например, `outputsVersion`, `formatsVersion`, `overlayFormatVersion`, `workspaceVersion`) и договориться о нумерации (например, 1.0).
3. Добавить док `docs/vtproj.md` с примерами минимальных файлов.


## Критерии приёмки

- [ ] В документации есть чёткий список файлов vtproj для MVP.
- [ ] Во всех JSON MVP есть соответствующий *Version.
- [ ] Примеры JSON валидны и запускаются.


## Как проверить

- Создать пустой vtproj по доке, запустить проект — старт без ошибок.


## Артефакты

- docs/vtproj.md
- Примеры JSON в vtproj/examples (или аналог)
