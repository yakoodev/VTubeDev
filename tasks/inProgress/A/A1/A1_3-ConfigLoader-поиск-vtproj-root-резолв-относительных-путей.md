# A1_3 — ConfigLoader: поиск vtproj root + резолв относительных путей

**Status:** ready for review  
**Owner:** Codex  
**Started:** 2026-02-10  
**Branch:** task/A1_3-configloader-vtproj-path-resolve


## Цель

Сделать загрузчик конфигов с резолвом относительных путей (mp4/png/dll), чтобы таски про оверлеи/видео не бодались с путями как с битриксом (битрикс — говно).


## Объём

**Включено:**

- IVtprojLocator
- IConfigLoader
- Резолв путей + проверки существования
- Читаемые ошибки


**Не включено:**

- Hot-reload/Watchers




## Шаги реализации

1. Стратегия поиска vtproj: аргумент командной строки / env / последнее значение из workspace.
2. Резолв путей: `vtprojRoot + relative` → absolute.
3. Если файла нет — ошибка с полным путём и тем, откуда он взялся.


## Критерии приёмки

- [ ] vtproj можно перенести в другую папку и всё продолжит грузиться (после указания пути).
- [ ] Отсутствующий asset даёт понятную ошибку (без молчаливого black screen).


## Как проверить

- Перенести vtproj, указать новый путь — успешно.
- Указать несуществующий mp4 — лог + понятное сообщение (UI при наличии debug панели).


## Артефакты

- ConfigLoader
- PathResolver
- Тесты резолва


## Отчёт выполнения

### Что сделано
- Добавлены `IVtprojLocator`, `IConfigLoader` и реализация поиска vtproj через `--vtproj`, `VT_PROJECT_PATH`, либо `workspace.json` (lastProjectPath).
- Добавлен `PathResolver` с резолвом относительных/абсолютных путей и читаемыми ошибками.
- Подключена DI-регистрация в `app`.
- Добавлены тесты резолва путей.

### Как проверить
1. `env NUGET_PACKAGES=/tmp/nuget-packages NUGET_HTTP_CACHE_PATH=/tmp/nuget-http-cache dotnet test app/app.sln`

### Коммиты
- TBD

### Риски / follow-ups
- Нет
