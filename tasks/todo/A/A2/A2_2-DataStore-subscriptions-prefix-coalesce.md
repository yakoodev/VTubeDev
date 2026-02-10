# A2_2 — DataStore: subscriptions + prefix + coalesce

**Status:** todo


## Цель

Подписки на изменения DataStore + coalesce, чтобы трекинг не "дудосил" UI.


## Объём

**Включено:**

- Subscribe exact/prefix
- Batch notify per frame
- Coalesce N ms


**Не включено:**

- Персистентность на диск




## Шаги реализации

1. Интерфейс подписки (IDisposable).
2. Уведомления батчами в конце кадра.
3. Coalesce режим (debounce) для частых обновлений.


## Критерии приёмки

- [ ] Prefix подписка ловит дочерние пути.
- [ ] При 1000 Set/сек подписчик получает ограниченное число уведомлений.


## Как проверить

- Смоук: эмулировать частые Set() и измерить число callback'ов.


## Артефакты

- Subscriptions
- Coalesce impl
- Smoke test
