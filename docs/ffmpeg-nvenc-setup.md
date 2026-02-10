# FFmpeg + NVENC (Windows) — локальная установка для MVP

Эта инструкция фиксирует, как поставить FFmpeg с NVENC и где хранить бинарники.

## Варианты

### Вариант A: Gyan.dev (рекомендуется)
Плюсы: популярный источник Windows-сборок, есть release и git-master варианты.citeturn0search0

### Вариант B: BtbN (автосборки)
Плюсы: ежедневные билды, удобные release assets.citeturn0search5

## Рекомендуемый путь установки

Держим бинарники вне репозитория:

```
C:\ThirdParty\FFmpeg\
```

## Шаги установки (Gyan.dev)

1. Скачать архив `ffmpeg-release-essentials.zip` или `ffmpeg-git-essentials.7z`.citeturn0search0
2. Распаковать в `C:\ThirdParty\FFmpeg\`.
3. Добавить путь к `ffmpeg.exe` в `.env` (локально, не в git).

Рекомендуемый ключ в `.env`:

```
FFMPEG_BIN=C:\ThirdParty\FFmpeg\bin\ffmpeg.exe
FFPROBE_BIN=C:\ThirdParty\FFmpeg\bin\ffprobe.exe
```

## Проверка NVENC

NVENC требует поддерживаемый GPU и драйвер.citeturn0search3

Проверка в PowerShell:

```
& C:\ThirdParty\FFmpeg\bin\ffmpeg.exe -hide_banner -encoders | findstr /i nvenc
```

Если NVENC доступен, в списке будут `h264_nvenc` и/или `hevc_nvenc`.

## Примечания

- FFmpeg бинарники не коммитим.
- FFmpeg поддерживает NVENC/NVDEC при наличии соответствующего драйвера и GPU.citeturn0search1
