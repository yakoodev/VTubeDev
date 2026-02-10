namespace app.Config;

public interface IPathResolver
{
    string ResolveFilePath(string rawPath, string baseDirectory, string source);
}
