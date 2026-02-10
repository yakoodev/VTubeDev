# B1_1 — RT_Base + RenderTargetPool + derived per formatId/profile

**Status:** todo


## Цель

Собрать костяк пайплайна: Base RT от камеры и derived RT под каждый formatId/profile (crop/scale), с пулом RT и debug-превью.


## Объём

**Включено:**

- RenderFormatProfile
- RT pool
- Crop/scale
- Debug preview


**Не включено:**

- Async GPU readback оптимизации




## Шаги реализации

1. Добавить модель `RenderFormatProfile` (size, crop rect, scale mode).
2. Сделать пул RenderTexture по ключу (w,h,format).
3. Собрать граф: Camera -> RT_Base -> Blit/Crop -> RT_Derived.
4. Добавить debug-окно/overlay для просмотра Base и Derived.


## Критерии приёмки

- [ ] Есть минимум 2 профиля (16:9 и 9:16) и они корректно рендерятся.
- [ ] RT не аллоцируются каждый кадр (пул используется).


## Как проверить

- Переключать profile во время работы — картинка меняется без лагов/аллоц спама.
- Сделать 2 скриншота derived для разных профилей.


## Артефакты

- RenderFormatProfile
- RenderTargetPool
- Debug preview UI
