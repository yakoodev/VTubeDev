using System.IO;

namespace app.Config;

public sealed class PathResolver : IPathResolver
{
    public string ResolveFilePath(string rawPath, string baseDirectory, string source)
    {
        if (string.IsNullOrWhiteSpace(rawPath))
        {
            throw new ConfigPathException($"[{source}] Empty path is not allowed.");
        }

        if (string.IsNullOrWhiteSpace(baseDirectory))
        {
            throw new ConfigPathException($"[{source}] Base directory is missing for path resolution.");
        }

        var resolvedPath = Path.IsPathRooted(rawPath)
            ? Path.GetFullPath(rawPath)
            : Path.GetFullPath(Path.Combine(baseDirectory, rawPath));

        if (!File.Exists(resolvedPath))
        {
            throw new ConfigPathException($"[{source}] File not found. Raw='{rawPath}', Resolved='{resolvedPath}', Base='{baseDirectory}'.");
        }

        return resolvedPath;
    }
}
