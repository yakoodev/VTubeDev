# VTube — архитектурный холст (MVP)

Этот документ — **единый источник правды** по решениям и контрактам. Всё ниже считается **принятым** для MVP (если не помечено как «позже»).

## Цели продукта
- Гибридный VTuber-стек: **2D + 3D** в одной сцене.
- Управление сценой/эффектами/постом и выдача результата в виде **web-эндпоинтов** для OBS + шаринг ссылкой.
- Расширяемость через **DLL-плагины** и настраиваемые графы обработки.

## Платформы
- MVP: **Windows**.
- Архитектура допускает расширение на macOS/Linux позже.

---

## 1) Рендер и композитинг (Unity URP)
### Движок
- **Unity + URP** — единственный рендер-пайплайн в MVP.

### Рендер-цепочка по камере
1) **Update (1 раз за кадр)**
- Data Graph: webcam → tracker → filters → `ParameterSet`.
- `bindings.json`: параметры → деформации/трансформы/материалы.
- Применяем очередь `SceneCommand`.

2) **Base 3D Render (URP)**
- Рендер 3D мира в `RT_Base(cameraId)`.

3) **2D/Hybrid Layer Pass**
- Рисуем слои персонажа/2D RenderItems (multi-plane) в `RT_Base`.
- Blend modes: `Normal/Add/Multiply/Screen`.
- Clipping: **1 маска** на RenderItem.
- Outline: включён в MVP.

4) **Silhouette Depth Write (по слоям)**
- Для слоёв с `silhouetteWrite=true` пишем depth по маске (alpha-clip) для перекрытия 2D↔3D.
- Режим по умолчанию: **Silhouette** (Strict/Off — позже).

5) **Post + UI Overlay (per format)**
- Постпроцесс применяется к **скомпоженному** кадру.
- Для каждого `formatId`:
  - если `derived=true`: crop/scale от `RT_Base`;
  - затем overlay UI по `overlayProfileId`.

6) **Outputs**
- web streaming endpoints (debug/release)
- окна вывода (для OBS Window Capture)
- replay encoder (если `bufferize=true`)

### Реализация порядка в URP
- Порядок обеспечиваем через **ScriptableRendererFeature/Pass** (или RenderGraph на новых версиях), чтобы было детерминировано и тестируемо.

---

## 2) Сцена: камеры, форматы, слои
### Камеры и форматы
- Камера — источник (`cameraId`).
- Формат — финальный кадр (`formatId`): crop/scale + overlay + output профили.
- Derived форматы не требуют отдельного 3D-рендера.

### Слои
- Сцена состоит из слоёв и подслоёв (для параллакса и т.п.).
- На слое могут быть 2D и 3D объекты.

---

## 3) UI/Overlays (per-format)
### Рендер-модель
- Камера рендерит сцену **без UI** → кадрирование под `formatId` → overlay per `formatId`.
- Профили вывода: **debug** и **release**.

### Технология
- MVP: **Unity UI overlay** (Canvas/UIToolkit) → `RT_Overlay(formatId)` → композит в output.
- Заложен интерфейс `IOverlayRenderer` + переключатель реализации (HTML overlay возможно позже).

### Управление
- `formats.json` хранит `overlayProfileId`.
- `SceneCommand`: `Format.SetOverlayProfile(formatId, overlayProfileId)`.

### Node editor (MVP)
- Node editor **только визуализирует** конфиг/граф (read-only).

---

## 4) Формат `overlay.json`
`ui/overlays/<overlayProfileId>/overlay.json` + `assets/` (картинки/шрифты).

### База
- `overlayFormatVersion: "1.0"`
- `design: { width, height }` — дизайн-разрешение.
- `scaling.mode: fit|fill|stretch` (рек. `fit`).
- `scaling.snapToPixels: bool`.
- `safeArea.default: { left, top, right, bottom }` в px дизайн-сетки.
- `widgets[]` с `RectTransform`-подобным `transform`:
  - `anchorMin/anchorMax`, `pivot` (0..1)
  - `posPx/sizePx` в дизайн-пикселях
- `tags[]` для debug/release.

### Debug vs Release
- Debug-виджеты помечаются `tags:["debug"]`.
- Debug-профиль включает debug-теги, release — нет.

### Data binding
- `bindings.textTemplate`, `bindings.valuePath`, `bindings.itemsPath`.
- DataStore пути: `scene.*`, `integrations.*`, `replay.*`, `vars.*`, `performance.*`, `system.*`.

### Шрифты
- У `Text` виджета можно указать `font` по имени (string).
- Резолвим через `assets/fonts/*` текущего overlayProfile.
- Если `font` не указан/не найден — используем дефолтный.

### Виджеты MVP
- `Text` (с `font`)
- `Image`
- `ProgressBar`
- `FeedList`
- `DebugHUD`

---

## 5) Вывод «просто трансляция» (MVP) — Streaming Endpoints
### Основная идея
- Отдаём финальную картинку **по `formatId`**, профили **debug/release**.
- По умолчанию доступ только localhost; remote включается явно (см. оркестратор).

### Endpoint’ы (MVP)
- `GET /view/<formatId>/<profile>` → HTML viewer (для OBS Browser Source и шаринга).
- `GET /stream/<formatId>/<profile>.mjpg` → MJPEG поток.
- `GET /snapshot/<formatId>/<profile>.png` → snapshot (превью/тесты).
- `GET /api/streams` → список стримов + URL.
- `GET /api/health`, `GET /api/metrics` → диагностика.

### Debug vs Release
- `debug` включает overlay-теги `tags:["debug"]`, может иметь ниже fps/качество.
- `release` без debug-тегов.

### Производительность (дефолты)
- `release`: 30 fps, `jpegQuality≈80`.
- `debug`: 15 fps, `jpegQuality≈60`.
- Slow-client policy: **дропаем старые кадры**, клиент получает самый свежий.

### On-demand
Формат/камера активны, если есть потребитель:
- соединение на `/stream/*` или `/view/*`
- окно вывода (Window Capture)
- `camera.bufferize=true` (replay)
Если потребителей нет → для этого формата **не рендерим и не кодируем**.

### OBS
- Основной путь: OBS Browser Source на `/view/<formatId>/release`.
- Фоллбек: отдельное окно вывода → OBS Window Capture.

### Авторизация при remote
- Токен в URL (Browser Source не любит заголовки).
- Всё логируется аудитом.

---

## 6) Output Profiles
### Хранение
- Профили выдачи храним отдельно: `outputs/profiles.json`.
- `formatId` ссылается на профиль (реюз между форматами/камерами).

### Параметры профиля (MVP)
- `fps`
- `jpegQuality`
- `maxViewers` (опц.)
- `idleTimeoutSec`
- `includeTags` (для debug)

### Snapshot
- Snapshot по умолчанию = **последний отрендеренный кадр**.

---

## 7) Web-orchestrator (UI + API)
- Десктоп поднимает локальный web-сервис.
- Роли: Owner / Moderator / Viewer.
- Инвайт-токены (Bearer), для Browser Source — токен в URL.
- Аудит-лог: кто/что/когда/результат.

---

## 8) `SceneCommand` v0.1
Единый язык управления сценой для UI/интеграций/плагинов.

MVP группы:
- `Layer.*` (enable/order/visibility)
- `Camera.*`, `Format.*` (вкл/профили/overlay)
- `Character.*` (params/expressions)
- `Fx.*` (trigger/enable)
- `Object.*` (spawn/despawn/set/attach)
- `Replay.*`

Очередь команд:
- `source`, `actorId`, `priority`, `timestamp`
- конфликты видны в логах

---

## 9) Плагины (DLL)
- Плагины = DLL, общий набор интерфейсов.
- На старте грузим через reflection, регистрируем в DI.
- `IPluginContext` даёт доступ к сервисам.
- Плагины могут менять логику через делегаты (если заложено в сервисах).
- Порядок плагинов настраиваем; конфликты фиксируются логами.
- Все вызовы через try/catch: «плагин пользователя — ответственность пользователя».

---

## 10) Интеграции и Rule Engine
### MVP источники
- Twitch chat
- Generic Webhook
- Hotkeys

### Поток
EventSource → Normalizer (`EventEnvelope`) → Rule Engine → `SceneCommand[]` → Audit.

`EventEnvelope`: `eventId/source/type/timestamp/actor/payload/tags`.

Секреты (tokens/keys) не кладём в проект: `.env`/локальное хранилище.

---

## 11) Трекинг
- MVP трекер: **MediaPipe**.
- Трекер выбирается/меняется через ноды Data Graph.
- Фильтры/сглаживание/маппинг — настраиваемо нодами.

---

## 12) Runtime vs Project state (сохранение/undo)
- Live изменения (порядок/видимость/переключения) по умолчанию **runtime-only**.
- `workspace.json` (локально) хранит рабочее состояние и **восстанавливается** при запуске.
- При **Save/Save As**: merge runtime overlay → project state (текущая «реальность» становится проектом).
- Undo/Redo — для проектных правок; live фиксируем аудитом.

Про runtime-объекты:
- По умолчанию ephemeral; при необходимости могут быть сохранены как template/prefab.

---

## 13) Templates/Prefabs и runtime-объекты
### Где лежат
- `assets/templates/`

### Что такое template
- Визуал + поведение.
- Props — **кастомные**, описаны в JSON (schema/типы/дефолты внутри template).
- Лимитов по умолчанию нет; пользователь может настроить сам.

Два пути (архитектурно):
- MVP: `Object.Spawn(templateId, ...properties...)`
- Позже: spawn «с нуля» (inline spec)

---

## 14) DataStore
- `dataStoreVersion: "1.0"`.
- Top-level: `scene`, `integrations`, `replay`, `vars`, `performance`, `system`.
- `vars.*` может писать кто угодно (rules/plugins/UI).
- «Пишем всё» гарантируется аудитом/логами; DataStore хранит данные для UI и может быть настраиваемым по объёму.

---

## 15) Replay / "мгновенный повтор"
- `camera.bufferize: bool`.
- `bufferize=true` → камера **всегда активна**.
- Ring buffer последних N секунд (по умолчанию 120).
- Буферим 1 формат на камеру (дефолт: release формат).
- Триггеры: UI + hotkey.
- Сохранение: папка пользователя (например Videos).
- Кодирование: **NVENC H.264 (AVC)**, контейнер предпочтительно **MKV**.

---

## 16) Тестирование (Codex + рендер)
- Быстрые тесты: engine-agnostic (.NET) — графы, rule engine, сериализация, очередь команд, плагины.
- Рендер-регрессии: рендер тест-проектов → PNG snapshots → сравнение с golden.
- Codex запускает тесты на ПК владельца; снимки получаем через snapshot endpoint или CLI-режим.

---

## 17) Packaging / дистрибуция
- Плагины: глобальные + проектные (оба пути).
- Секреты: `.env` (не в `vtproj`).
- `workspace.json`: локально.
- `vtproj` как папка + **Export as `.vtproj`** (архив).
- Миграции форматов и проверка минимальной версии приложения при открытии проекта: **да**.

---

## Открытые пункты (можно закрыть позже)
- Примеры минимального `vtproj` (reference project) — полезно для онбординга и агентов.
- Детализация формата `template.json` и `outputs/profiles.json` (примеры/схемы).
- Полный список гарантированных путей DataStore (табличкой).
