namespace app.Config;

public sealed record VtprojProjectContext(
    string RootPath,
    string RootSource,
    string? WorkspacePath
);
