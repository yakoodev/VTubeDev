# Unity ↔ App Boundary (Contract v0.1)

Этот документ фиксирует границу между Unity‑рендером и HTTP‑сервисом (app). Цель — единый контракт обмена командами, кадрами и статусами, чтобы Unity и app развивались независимо.

## 0) Кратко
- **App** отвечает за HTTP (`/view`, `/stream`, `/snapshot`, `/api/*`, `/cmd`) и раздачу данных.
- **Unity** отвечает за рендер, композитинг, DataStore и выполнение `SceneCommand`.
- Связь по **локальному транспорту** (Windows): **Named Pipe** для команд и **Memory‑Mapped File** (MMF) для кадров.

## 1) Transport (рекомендованный)

### 1.1 Команды (App → Unity)
- Transport: **Named Pipe**
- Название pipe: `vtube_cmd_v1`
- Формат сообщения: **JSON**, одна команда на сообщение.
- Ответ: ack JSON (accepted/rejected + reason).

### 1.2 Кадры (Unity → App)
- Transport: **Memory‑Mapped File** (MMF) + named mutex/semaphore.
- MMF имя: `vtube_frames_v1`
- Сигнализация: `vtube_frames_signal_v1`
- Модель: ring‑buffer из N слотов (по форматам/профилям).

## 2) Data Types

### 2.1 SceneCommand (App → Unity)
```json
{
  "requestId": "req_123",
  "source": "web",
  "timestampUtc": "2026-02-10T00:00:00Z",
  "command": {
    "type": "SetInputSource",
    "payload": { "mode": "VideoSim" }
  }
}
```

Ответ (Unity → App):
```json
{
  "status": "accepted",
  "requestId": "req_123",
  "error": null
}
```

### 2.2 Frame Header (Unity → App)
Каждый слот в MMF содержит заголовок и байтовые данные кадра.

```json
{
  "formatId": "cam_main_16x9",
  "profile": "release",
  "width": 1920,
  "height": 1080,
  "pixelFormat": "RGBA32",
  "frameIndex": 12450,
  "timestampUtc": "2026-02-10T00:00:00Z"
}
```

Payload кадра: raw RGBA32 (по умолчанию для MVP), допускается JPEG/PNG как вариант оптимизации.

## 3) Lifecycle

1. App стартует и открывает pipe `vtube_cmd_v1`.
2. Unity при старте подключается к pipe и готовится принимать команды.
3. Unity создаёт MMF `vtube_frames_v1` и сигнал `vtube_frames_signal_v1`.
4. App подписывается на сигнал и читает последний доступный кадр.

## 4) Ошибки и деградация
- Если pipe недоступен — `/cmd` возвращает 503.
- Если MMF не создан — `/stream` и `/snapshot` возвращают 503.
- Любая ошибка не должна валить Unity или app; логировать и продолжать.

## 5) Совместимость и версия
- Версия контракта: **v0.1**.
- При изменении формата сообщения/слота — повышаем версию имени (`vtube_cmd_v2` / `vtube_frames_v2`).

## 6) Безопасность
- Только **localhost**, без внешней экспозиции по умолчанию.
- Авторизация (токен) применяется на уровне HTTP, не на уровне локального транспорта.

## 7) Следующие шаги
- Реализация прототипа: named pipe + MMF.
- Добавление health‑статуса Unity в `/api/health`.
- Ограничение частоты чтения в app (fps cap).
