# D1_2 — Viewer: /view/<formatId>/<profile> (HTML) для OBS Browser Source

**Status:** todo


## Цель

## Контекст / зависимости

- Требует рабочие /stream и /snapshot (D2_2, D2_1).

Сделать viewer HTML страницу, чтобы OBS мог брать Browser Source напрямую (и агент мог сделать скрин из браузера).


## Объём

**Включено:**

- HTML template
- Отображение MJPEG или snapshot poll
- Debug info (formatId/profile/fps)


**Не включено:**

- Сборка фронта




## Шаги реализации

1. Endpoint отдаёт простой HTML.
2. Внутри подключается MJPEG stream или snapshot refresh (fallback).
3. Показывает текущий formatId/profile и размер.


## Критерии приёмки

- [ ] Страница открывается и показывает картинку.
- [ ] При смене formatId/profile меняется размер/контент.


## Как проверить

- Открыть в браузере /view/default/default — сделать скриншот страницы.


## Артефакты

- /view endpoint
- HTML template
