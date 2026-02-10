# G1_2 — VideoSimFrameSource: avatar-sim.mp4 → Texture (Unity VideoPlayer)

**Status:** todo


## Цель

Реализовать источник видео-заглушки из `VTubeDev\tasks\avatar\avatar-sim.mp4` через Unity VideoPlayer.


## Объём

**Включено:**

- VideoPlayer
- Path resolve через ConfigLoader
- Loop
- Frame доступен как Texture


**Не включено:**

- Точная синхронизация по таймкоду

## Контекст / зависимости

- Резолв пути через ConfigLoader (A1_3).




## Шаги реализации

1. Реализовать VideoSimFrameSource.
2. Путь берётся из конфига, дефолт — указанный путь (если существует).
3. Loop + автозапуск.


## Критерии приёмки

- [ ] Видео воспроизводится и обновляет кадры.
- [ ] Если файла нет — понятная ошибка и fallback.


## Как проверить

- В debug UI видно, что кадры обновляются (например, frameCounter растёт).
- Скриншот кадра.


## Артефакты

- VideoSimFrameSource
- Config key: videoSimPath
