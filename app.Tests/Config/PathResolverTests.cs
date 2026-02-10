using System;
using System.IO;
using app.Config;
using Xunit;

namespace app.Tests.Config;

public class PathResolverTests
{
    [Fact]
    public void ResolveFilePath_UsesBaseDirectoryForRelativePaths()
    {
        var resolver = new PathResolver();
        var root = CreateTempDir();
        var filePath = Path.Combine(root, "asset.png");
        File.WriteAllText(filePath, "data");

        var resolved = resolver.ResolveFilePath("asset.png", root, "test");

        Assert.Equal(Path.GetFullPath(filePath), resolved);
    }

    [Fact]
    public void ResolveFilePath_AllowsAbsolutePaths()
    {
        var resolver = new PathResolver();
        var root = CreateTempDir();
        var filePath = Path.Combine(root, "clip.mp4");
        File.WriteAllText(filePath, "data");

        var resolved = resolver.ResolveFilePath(filePath, root, "test");

        Assert.Equal(Path.GetFullPath(filePath), resolved);
    }

    [Fact]
    public void ResolveFilePath_ThrowsWithReadableErrorOnMissingFile()
    {
        var resolver = new PathResolver();
        var root = CreateTempDir();
        var missing = "missing.dll";

        var exception = Assert.Throws<ConfigPathException>(() => resolver.ResolveFilePath(missing, root, "widget.icon"));

        Assert.Contains("missing.dll", exception.Message, StringComparison.Ordinal);
        Assert.Contains("widget.icon", exception.Message, StringComparison.Ordinal);
    }

    private static string CreateTempDir()
    {
        var path = Path.Combine(Path.GetTempPath(), "vtube-tests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(path);
        return path;
    }
}
