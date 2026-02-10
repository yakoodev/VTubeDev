namespace app.Config;

public sealed class ConfigLoader : IConfigLoader
{
    private readonly IVtprojLocator _vtprojLocator;
    private readonly IPathResolver _pathResolver;

    public ConfigLoader(IVtprojLocator vtprojLocator, IPathResolver pathResolver)
    {
        _vtprojLocator = vtprojLocator;
        _pathResolver = pathResolver;
    }

    public VtprojProjectContext LoadProject()
    {
        var result = _vtprojLocator.Locate();
        if (!result.Found || string.IsNullOrWhiteSpace(result.RootPath))
        {
            var error = result.Error ?? "vtproj root not resolved.";
            throw new ConfigPathException(error);
        }

        return new VtprojProjectContext(result.RootPath, result.Source ?? "unknown", result.WorkspacePath);
    }

    public string ResolveAssetPath(string rawPath, string source)
    {
        var context = LoadProject();
        var origin = $"{source} (vtproj={context.RootSource})";
        return _pathResolver.ResolveFilePath(rawPath, context.RootPath, origin);
    }
}
