# VTube — TASKS INDEX (2026-02-07)

Все таски лежат в папках по этапам: A..I + X.

## Список

- **A1.1** — Структура репозитория и соглашения по путям  
  `A/A1/A1_1-Структура-репозитория-и-соглашения-по-путям.md`
- **A1.2** — DI контейнер и composition root  
  `A/A1/A1_2-DI-контейнер-и-composition-root.md`
- **A1.3** — Конфиг-лоадер + проверка версий схем  
  `A/A1/A1_3-Конфиг-лоадер-проверка-версий-схем.md`
- **A1.4** — Логирование + requestId (runtime + web)  
  `A/A1/A1_4-Логирование-requestId-runtime-web.md`
- **A2.1** — DataStore v1: модель + get/set по path  
  `A/A2/A2_1-DataStore-v1-модель-get-set-по-path.md`
- **A2.2** — DataStore: подписки и coalesce  
  `A/A2/A2_2-DataStore-подписки-и-coalesce.md`
- **A2.3** — DataStore: Snapshot() в JSON + опции лимитера  
  `A/A2/A2_3-DataStore-Snapshot-в-JSON-опции-лимитера.md`
- **A3.1** — workspace.json: схема + merge runtime→project  
  `A/A3/A3_1-workspace-json-схема-merge-runtime-project.md`
- **A3.2** — Undo/Redo для project state  
  `A/A3/A3_2-Undo-Redo-для-project-state.md`
- **A3.3** — Audit log: каркас и запись SceneCommand  
  `A/A3/A3_3-Audit-log-каркас-и-запись-SceneCommand.md`
- **B1.1** — CameraRegistry: камераId и конфиги  
  `B/B1/B1_1-CameraRegistry-камераId-и-конфиги.md`
- **B1.2** — FormatRegistry: formatId + derived  
  `B/B1/B1_2-FormatRegistry-formatId-derived.md`
- **B1.3** — Layers/SubLayers: модель и порядок  
  `B/B1/B1_3-Layers-SubLayers-модель-и-порядок.md`
- **B2.1** — URP RendererFeature: каркас pass’ов и порядок  
  `B/B2/B2_1-URP-RendererFeature-каркас-pass-ов-и-порядок.md`
- **B2.2** — RT_Base(cameraId): аллокация и пул  
  `B/B2/B2_2-RT_Base-cameraId-аллокация-и-пул.md`
- **B2.3** — Hybrid2D: RenderItems + blend modes  
  `B/B2/B2_3-Hybrid2D-RenderItems-blend-modes.md`
- **B2.4** — Clipping: 1 маска на RenderItem  
  `B/B2/B2_4-Clipping-1-маска-на-RenderItem.md`
- **B2.5** — Outline (MVP)  
  `B/B2/B2_5-Outline-MVP.md`
- **B2.6** — Silhouette Depth Write (default)  
  `B/B2/B2_6-Silhouette-Depth-Write-default.md`
- **B2.7** — Per-format композитор: crop/scale + overlay + output  
  `B/B2/B2_7-Per-format-композитор-crop-scale-overlay-output.md`
- **B3.1** — Data Graph: INode + GraphRunner  
  `B/B3/B3_1-Data-Graph-INode-GraphRunner.md`
- **B3.2** — ParameterSet  
  `B/B3/B3_2-ParameterSet.md`
- **B3.3** — bindings.json: параметры → Transform/Material  
  `B/B3/B3_3-bindings-json-параметры-Transform-Material.md`
- **B3.4** — Порядок Update: Graph → Bindings → Commands  
  `B/B3/B3_4-Порядок-Update-Graph-Bindings-Commands.md`
- **C1.1** — overlay.json v1.0: парсер и модели  
  `C/C1/C1_1-overlay-json-v1-0-парсер-и-модели.md`
- **C1.2** — Теги debug/release  
  `C/C1/C1_2-Теги-debug-release.md`
- **C1.3** — Overlay assets: fonts/images resolver  
  `C/C1/C1_3-Overlay-assets-fonts-images-resolver.md`
- **C2.1** — IOverlayRenderer + Unity реализация  
  `C/C2/C2_1-IOverlayRenderer-Unity-реализация.md`
- **C2.2** — Layout engine: anchors/pivot + scaling fit/fill/stretch  
  `C/C2/C2_2-Layout-engine-anchors-pivot-scaling-fit-fill-stretch.md`
- **C2.3** — Виджеты MVP реализация  
  `C/C2/C2_3-Виджеты-MVP-реализация.md`
- **C3.1** — Overlay binding: valuePath/itemsPath резолвер  
  `C/C3/C3_1-Overlay-binding-valuePath-itemsPath-резолвер.md`
- **C3.2** — TextTemplate: мини-шаблонизатор  
  `C/C3/C3_2-TextTemplate-мини-шаблонизатор.md`
- **C3.3** — DebugHUD виджет  
  `C/C3/C3_3-DebugHUD-виджет.md`
- **D1.1** — HTTP сервер (localhost) + порт  
  `D/D1/D1_1-HTTP-сервер-localhost-порт.md`
- **D1.2** — /api/metrics endpoint  
  `D/D1/D1_2-api-metrics-endpoint.md`
- **D2.1** — /api/streams  
  `D/D2/D2_1-api-streams.md`
- **D2.2** — /snapshot PNG  
  `D/D2/D2_2-snapshot-PNG.md`
- **D2.3** — /stream MJPEG  
  `D/D2/D2_3-stream-MJPEG.md`
- **D2.4** — /view HTML viewer  
  `D/D2/D2_4-view-HTML-viewer.md`
- **D3.1** — Output profiles loader  
  `D/D3/D3_1-Output-profiles-loader.md`
- **D3.2** — Idle timeout + maxViewers  
  `D/D3/D3_2-Idle-timeout-maxViewers.md`
- **D4.1** — On-demand activation (consumers + ref-count)  
  `D/D4/D4_1-On-demand-activation-consumers-ref-count.md`
- **D4.2** — Окно вывода per-format  
  `D/D4/D4_2-Окно-вывода-per-format.md`
- **E1.1** — Роли и права  
  `E/E1/E1_1-Роли-и-права.md`
- **E1.2** — Invite tokens + токен в URL  
  `E/E1/E1_2-Invite-tokens-токен-в-URL.md`
- **E1.3** — Audit по API  
  `E/E1/E1_3-Audit-по-API.md`
- **E2.1** — UI: список форматов/стримов  
  `E/E2/E2_1-UI-список-форматов-стримов.md`
- **E2.2** — UI: переключение overlayProfileId  
  `E/E2/E2_2-UI-переключение-overlayProfileId.md`
- **E2.3** — Read-only Node editor  
  `E/E2/E2_3-Read-only-Node-editor.md`
- **F1.1** — SceneCommand v0.1: спецификация и схемы  
  `F/F1/F1_1-SceneCommand-v0-1-спецификация-и-схемы.md`
- **F1.2** — SceneCommandQueue: priority + конфликты  
  `F/F1/F1_2-SceneCommandQueue-priority-конфликты.md`
- **F1.3** — SceneCommandExecutor  
  `F/F1/F1_3-SceneCommandExecutor.md`
- **F2.1** — EventEnvelope контракт  
  `F/F2/F2_1-EventEnvelope-контракт.md`
- **F2.2** — Normalizer: Twitch/Webhook/Hotkeys  
  `F/F2/F2_2-Normalizer-Twitch-Webhook-Hotkeys.md`
- **F3.1** — Rule Engine: формат правил + загрузка  
  `F/F3/F3_1-Rule-Engine-формат-правил-загрузка.md`
- **F3.2** — Rule Engine pipeline + audit  
  `F/F3/F3_2-Rule-Engine-pipeline-audit.md`
- **F4.1** — Twitch chat интеграция  
  `F/F4/F4_1-Twitch-chat-интеграция.md`
- **F4.2** — Webhook интеграция  
  `F/F4/F4_2-Webhook-интеграция.md`
- **F4.3** — Hotkeys интеграция  
  `F/F4/F4_3-Hotkeys-интеграция.md`
- **G1.1** — Webcam input node  
  `G/G1/G1_1-Webcam-input-node.md`
- **G1.2** — MediaPipe tracker node  
  `G/G1/G1_2-MediaPipe-tracker-node.md`
- **G1.3** — Filter nodes (low-pass/clamp/deadzone)  
  `G/G1/G1_3-Filter-nodes-low-pass-clamp-deadzone.md`
- **H1.1** — Ring buffer (120s)  
  `H/H1/H1_1-Ring-buffer-120s.md`
- **H1.2** — Replay.Trigger: UI + hotkey  
  `H/H1/H1_2-Replay-Trigger-UI-hotkey.md`
- **H2.1** — NVENC H.264 encoder  
  `H/H2/H2_1-NVENC-H-264-encoder.md`
- **H2.2** — Сохранение replay: Videos + naming + audit  
  `H/H2/H2_2-Сохранение-replay-Videos-naming-audit.md`
- **I1.1** — Unit tests: конфиги (overlay/profiles/graph/commands)  
  `I/I1/I1_1-Unit-tests-конфиги-overlay-profiles-graph-commands.md`
- **I1.2** — Unit tests: Rule Engine + queue conflicts  
  `I/I1/I1_2-Unit-tests-Rule-Engine-queue-conflicts.md`
- **I2.1** — Рендер-регрессии: headless прогон + snapshot  
  `I/I2/I2_1-Рендер-регрессии-headless-прогон-snapshot.md`
- **I2.2** — Сравнение с golden (tolerance)  
  `I/I2/I2_2-Сравнение-с-golden-tolerance.md`
- **I3.1** — Export/Import .vtproj (архив)  
  `I/I3/I3_1-Export-Import-vtproj-архив.md`
- **I3.2** — Migrations: minAppVersion + каркас  
  `I/I3/I3_2-Migrations-minAppVersion-каркас.md`
- **I3.3** — Plugin loader: global + project paths  
  `I/I3/I3_3-Plugin-loader-global-project-paths.md`
- **X1.1** — Reference vtproj: 1 камера + 2 формата  
  `X/X1/X1_1-Reference-vtproj-1-камера-2-формата.md`
- **X1.2** — Reference overlayProfile: debug/release теги  
  `X/X1/X1_2-Reference-overlayProfile-debug-release-теги.md`
- **X2.1** — Док: подключение OBS  
  `X/X2/X2_1-Док-подключение-OBS.md`
