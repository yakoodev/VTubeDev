# C1_1 — Overlay профили: загрузка из vtproj + resolver по formatId/profile

**Status:** todo


## Цель

Ввести overlay профили (overlayProfileId) и резолв по formatId, чтобы для каждого формата можно было свой UI.


## Объём

**Включено:**

- overlay schema
- OverlayRegistry
- Resolver outputs->overlay
- Подключение к композиту


**Не включено:**

- Редактор overlay внутри приложения




## Шаги реализации

1. Определить overlay JSON: list widgets, layout, bindings, z-order.
2. Сделать registry id->profile (из vtproj).
3. В formats.json добавить поле overlayProfileId и резолвить его по formatId.


## Критерии приёмки

- [ ] Разные formatId/profile могут иметь разные overlay.
- [ ] Overlay меняется без перезапуска (через команду/перезагрузку конфигов).


## Как проверить

- Сделать 2 overlay: text vs image; переключить profile и сделать 2 скрина.


## Артефакты

- Overlay schema
- Registry/Resolver
- Примеры overlay
