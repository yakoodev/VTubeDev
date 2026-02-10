# C1_2 — Overlay layout MVP: anchors + safe-area + z-order

**Status:** todo


## Цель

Сделать простой layout движок: anchors, offsets, safe-area, z-order. Никакой магии — только чтобы работало стабильно на разных разрешениях.


## Объём

**Включено:**

- Anchors TL/TR/BL/BR/C
- Offsets px/%
- Safe-area
- Z-order


**Не включено:**

- Flex/Grid полноценные




## Шаги реализации

1. Определить координатную систему overlay (W,H).
2. Реализовать anchors и offsets.
3. Добавить safe-area из конфига.
4. Z-order сортировка виджетов при рендере.


## Критерии приёмки

- [ ] Виджеты не 'уплывают' при смене разрешения.
- [ ] Safe-area реально отступает.


## Как проверить

- Запустить 16:9 и 9:16, сделать скрины с одинаковым overlay.


## Артефакты

- Layout engine
- Пример overlay с anchors
