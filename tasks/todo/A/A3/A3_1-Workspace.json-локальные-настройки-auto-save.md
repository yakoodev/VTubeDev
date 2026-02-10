# A3_1 — Workspace.json: локальные настройки + auto-save

**Status:** todo


## Цель

Сделать workspace.json для локальных настроек (последний vtproj, выбранный input mode, выбранный output profile), без попадания в git.


## Объём

**Включено:**

- Модель + schema
- Load on start
- Save debounced
- Проверка gitignore


**Не включено:**

- Синк между ПК




## Шаги реализации

1. Определить модель workspace + workspaceVersion.
2. Загрузка при старте; сохранение при изменениях (debounced).
3. Убедиться, что workspace.json не трекается git.


## Критерии приёмки

- [ ] Настройки восстанавливаются после перезапуска.
- [ ] workspace.json не появляется в git status.


## Как проверить

- Поменять input mode на VideoSim, перезапустить — сохранилось.
- `git status` чистый.


## Артефакты

- Workspace model/schema
- Load/Save code
