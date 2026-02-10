# C2_1 — Widgets: Text/Image/Bar + bindings к DataStore

**Status:** todo


## Цель

Сделать 3 базовых виджета и биндинг к DataStore, чтобы можно было показывать данные и проверять команды.


## Объём

**Включено:**

- Text шаблоны
- Image png
- Bar 0..1
- Fallback при ошибках path


**Не включено:**

- Анимации и сложные шрифты




## Шаги реализации

1. Text: шаблонные вставки из DataStore (например {path:/stats/hp}).
2. Image: загрузка png по относительному пути vtproj.
3. Bar: float 0..1 + clamp + (опционально) smoothing.


## Критерии приёмки

- [ ] Изменение DataStore отражается на overlay в течение 1 кадра (или coalesce).
- [ ] Неверный path не ломает рендер.


## Как проверить

- Отправить SceneCommand SetData — увидеть обновление текста и бара; сделать скрин.


## Артефакты

- Widgets
- Binding system
- Примеры конфигов
