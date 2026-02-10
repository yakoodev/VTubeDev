# A1_4 — Логирование: requestId + источники (ui/web/hotkeys)

Status: inProgress  
Owner: Codex  
Started: 2026-02-10  
Branch: task/A1_4-logging-requestid-sources


## Цель

Единый лог с requestId, чтобы по одному идентификатору собрать историю команды (кто нажал, что применилось, что отрендерилось).


## Объём

**Включено:**

- RequestContext
- Префикс/structured лог
- Проброс requestId в web ответы


**Не включено:**

- Distributed tracing




## Шаги реализации

1. Ввести RequestContext (requestId, source, ts).
2. Все SceneCommand обязаны нести RequestContext.
3. Логи пишут requestId и source.
4. Web endpoints возвращают requestId.


## Критерии приёмки

- [ ] Любая команда имеет requestId в логах.
- [ ] Команды из web и UI различаются по source.


## Как проверить

- POST /cmd → в ответе requestId; в Unity-консоли этот requestId встречается минимум в 2 местах (enqueue + apply).


## Артефакты

- RequestContext
- Log adapter
- Изменения web DTO

## Отчёт выполнения

Что сделано:
- Добавлен `RequestContext` и прокинут в `SceneCommand`/`SceneCommandRequest`.
- Нормализация `requestId/source/timestampUtc` в `/cmd` + запись логов с `requestId` и `source`.
- Обновлён пример запроса в `app.http` под `context`.

Как проверить:
- `dotnet test app.Tests/app.Tests.csproj`
- `POST /cmd` → в ответе `requestId`, в логах есть `SceneCommand enqueued ... requestId=... source=...`

Коммиты:
- (будут добавлены после фиксации)

Риски/заметки:
- Тесты не прошли из-за `Permission denied` на NuGet кеш (см. вывод `dotnet test`).
