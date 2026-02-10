# A1_2 — JSON Schema: scene/outputs/overlay/workspace (MVP)

**Status:** ready for review
**Owner:** Codex
**Started:** 2026-02-10
**Branch:** task/A1_2-json-schema-mvp


## Цель

Добавить JSON Schema для MVP-конфигов и валидатор (Editor + runtime), чтобы агент ловил кривые конфиги до того, как они превратятся в 'почему чёрный экран?'.


## Объём

**Включено:**

- Схемы для MVP
- Валидация в Editor menu
- Runtime проверка на старте (с понятными ошибками)


**Не включено:**

- Глубокая валидация семантики (например, существование всех assetId) — частично




## Шаги реализации

1. Создать папку со схемами (например `Assets/VTube/Schemas/`).
2. Сделать Editor пункт меню `VTube/Validate Configs`.
3. На старте приложения валидировать загруженные конфиги и падать/останавливать пайплайн с понятным сообщением.


## Критерии приёмки

- [ ] Битый JSON ловится валидатором (с указанием поля и пути).
- [ ] Корректные примеры проходят.
- [ ] Ошибки видны в консоли Unity; в app-логах — если web уже подключён.


## Как проверить

- Сломать тип поля в overlay.json — получить ошибку со строкой/путём.
- Починить — ошибок нет.


## Артефакты

- *.schema.json
- Validator code
- Docs: как валидировать

## Отчёт выполнения

Что сделано:
- Добавлены JSON Schemas для `scene.json`, `outputs.json`, `overlay.json`, `workspace.json`.
- Реализован валидатор (subset JSON Schema: type/required/properties/additionalProperties/items/enum/min/max).
- Runtime‑проверка на старте через `VT_PROJECT_PATH` или `--vtproj`, остановка playmode при ошибках.
- Editor меню `VTube/Validate Configs` с возможностью выбрать `vtproj` и `workspace.json`.
- Документация по валидации в `docs/vtproj.md`.

Как проверить:
1. В Unity Editor открыть меню `VTube/Validate Configs` и выбрать `examples/vtproj-minimal/vtproj`.
2. (Опционально) выбрать `workspace.json` или пропустить.
3. Сломать тип поля в `examples/vtproj-minimal/vtproj/ui/overlays/default/overlay.json` и повторить валидацию — должна быть ошибка с путём.

Коммиты:
- `5894d47` feat(A1_2): add config schemas and validator
- `163bebd` docs(A1_2): add task report

Риски / follow-ups:
- Runtime‑валидация требует явного пути к `vtproj` (env/argv); при отсутствии пути проверка пропускается.

PR:
- `gh pr create` не выполнен (нет авторизации). Создать PR вручную: `https://github.com/yakoodev/VTubeDev/pull/new/task/A1_2-json-schema-mvp`.
