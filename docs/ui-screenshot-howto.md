# UI Screenshot How-To (Unity Play Mode)

This project uses a Screen Space - Overlay Canvas, so camera-based RenderTexture grabs do NOT include UI.
Use one of the two methods below when you need the UI visible in the image.

## Recommended: `ScreenCapture.CaptureScreenshotAsTexture`
Use this when you want an immediate in-memory capture (best for automation).

```csharp
var dir = @"C:/Users/Yakoo/source/repos/VTubeDev/tmp/vtube-temp";
Directory.CreateDirectory(dir);
var path = Path.Combine(dir, "mainCamera_with_ui.png");

var tex = ScreenCapture.CaptureScreenshotAsTexture();
if (tex == null) throw new Exception("CaptureScreenshotAsTexture returned null");
var bytes = tex.EncodeToPNG();
UnityEngine.Object.Destroy(tex);

File.WriteAllBytes(path, bytes);
```

## Alternative: `ScreenCapture.CaptureScreenshot`
This writes to disk asynchronously. Wait a short time before reading the file.

```csharp
var dir = @"C:/Users/Yakoo/source/repos/VTubeDev/tmp/vtube-temp";
Directory.CreateDirectory(dir);
var path = Path.Combine(dir, "mainCamera_with_ui.png");

ScreenCapture.CaptureScreenshot(path, 1);
// Wait 1-2 seconds before reading the file
```

## Why camera RenderTexture screenshots miss UI
`Camera.Render()` does not include `ScreenSpaceOverlay` Canvas.
If you must use camera renders, switch Canvas to `ScreenSpaceCamera` and assign the camera.
