# A2_1 — DataStore v1: typed values + get/set по path

**Status:** todo


## Цель

Сделать DataStore как центральный 'карман' данных для биндингов (overlay, команды, фильтры).


## Объём

**Включено:**

- DataValue тип
- Get/Set
- Ошибки типов
- Сериализация (минимально)


**Не включено:**

- Подписки/coalesce




## Шаги реализации

1. Определить DataValue (bool/int/float/string/object).
2. Сделать Set(path,value,ctx) и Get(path).
3. Типовые конфликты должны логироваться и не падать.


## Критерии приёмки

- [ ] Set/Get работают на базовых типах.
- [ ] Неверный тип не валит приложение.


## Как проверить

- Editor test: set float → get float ok; set float → get string возвращает null/ошибку.


## Артефакты

- DataStore v1
- Tests
