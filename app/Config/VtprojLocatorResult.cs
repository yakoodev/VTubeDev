namespace app.Config;

public sealed record VtprojLocatorResult(
    bool Found,
    string? RootPath,
    string? Source,
    string? WorkspacePath,
    string? Error
)
{
    public static VtprojLocatorResult Success(string rootPath, string source, string? workspacePath)
        => new(true, rootPath, source, workspacePath, null);

    public static VtprojLocatorResult Failure(string error, string? workspacePath = null)
        => new(false, null, null, workspacePath, error);
}
