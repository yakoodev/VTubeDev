# FFmpeg + NVENC (Windows) — локальная установка для MVP

Эта инструкция фиксирует, как поставить FFmpeg с NVENC и где хранить бинарники.

## Варианты

### Вариант A: Gyan.dev (рекомендуется)
Плюсы: популярный источник Windows-сборок, есть release и git-master варианты.

### Вариант B: BtbN (автосборки)
Плюсы: ежедневные билды, удобные release assets.

## Рекомендуемый способ

Используем `ffmpeg` из PATH (например, установленный через WinGet). Если PATH недоступен, можно указать явный путь через `.env`.

## Шаги установки (Gyan.dev)

1. Скачать архив `ffmpeg-release-essentials.zip` или `ffmpeg-git-essentials.7z`.
2. Распаковать в удобную папку (например, `C:\ffmpeg\`) и добавить `bin` в PATH.
3. Проверить, что `ffmpeg` запускается из PowerShell.

Если PATH недоступен, добавить в `.env` (локально, не в git):

```
FFMPEG_BIN=C:\ffmpeg\ffmpeg.exe
FFPROBE_BIN=C:\ffmpeg\ffprobe.exe
```

## Проверка NVENC

NVENC требует поддерживаемый GPU и драйвер.

Проверка в PowerShell:

```
& ffmpeg -hide_banner -encoders | findstr /i nvenc
```

Если NVENC доступен, в списке будут `h264_nvenc` и/или `hevc_nvenc`.

## Примечания

- FFmpeg бинарники не коммитим.
- FFmpeg поддерживает NVENC/NVDEC при наличии соответствующего драйвера и GPU.
