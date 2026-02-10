# F1_2 — POST /cmd: endpoint для SceneCommand + requestId

**Status:** todo


## Цель

## Контекст / зависимости

- Требует SceneCommand и очередь (F1_1).
- Рекомендуется наличие requestId logging (A1_4).

Сделать web endpoint для отправки команд в очередь и получать requestId в ответ.


## Объём

**Включено:**

- POST /cmd
- 400 validation
- requestId in response
- rate limit minimal


**Не включено:**

- Auth




## Шаги реализации

1. DTO команд в JSON.
2. Валидация тела и ограничение размера.
3. Возврат requestId и статуса (accepted/rejected).


## Критерии приёмки

- [ ] /cmd принимает команды и меняет сцену.
- [ ] Ошибки валидации понятны.


## Как проверить

- Отправить SetInputSource(VideoSim) — источник переключился.


## Артефакты

- /cmd endpoint
- DTO/validation
