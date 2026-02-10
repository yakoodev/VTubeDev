using System.IO;
using System.Text.Json;

namespace app.Config;

public sealed class VtprojLocator : IVtprojLocator
{
    private readonly string[] _args;

    public VtprojLocator(string[] args)
    {
        _args = args;
    }

    public VtprojLocatorResult Locate()
    {
        var argPath = TryGetArgValue("--vtproj");
        if (!string.IsNullOrWhiteSpace(argPath))
        {
            return ValidateRoot(argPath, "arg:--vtproj", ResolveWorkspacePath());
        }

        var envPath = Environment.GetEnvironmentVariable("VT_PROJECT_PATH");
        if (!string.IsNullOrWhiteSpace(envPath))
        {
            return ValidateRoot(envPath, "env:VT_PROJECT_PATH", ResolveWorkspacePath());
        }

        var workspacePath = ResolveWorkspacePath();
        if (!string.IsNullOrWhiteSpace(workspacePath))
        {
            var workspaceResult = TryGetWorkspaceLastProject(workspacePath);
            if (workspaceResult.Found && !string.IsNullOrWhiteSpace(workspaceResult.RootPath))
            {
                return ValidateRoot(workspaceResult.RootPath, "workspace:lastProjectPath", workspacePath);
            }

            return VtprojLocatorResult.Failure(
                workspaceResult.Error ?? "workspace.json did not include lastProjectPath.",
                workspacePath);
        }

        return VtprojLocatorResult.Failure("vtproj path not provided (use --vtproj, VT_PROJECT_PATH, or workspace.json).", null);
    }

    private static VtprojLocatorResult ValidateRoot(string rootPath, string source, string? workspacePath)
    {
        var fullPath = Path.GetFullPath(rootPath);
        if (!Directory.Exists(fullPath))
        {
            return VtprojLocatorResult.Failure($"vtproj root not found. Source='{source}', Path='{fullPath}'.", workspacePath);
        }

        return VtprojLocatorResult.Success(fullPath, source, workspacePath);
    }

    private string? ResolveWorkspacePath()
    {
        var envPath = Environment.GetEnvironmentVariable("VT_WORKSPACE_PATH");
        if (!string.IsNullOrWhiteSpace(envPath))
        {
            return envPath;
        }

        return TryGetArgValue("--workspace");
    }

    private VtprojLocatorResult TryGetWorkspaceLastProject(string workspacePath)
    {
        if (!File.Exists(workspacePath))
        {
            return VtprojLocatorResult.Failure($"workspace.json not found: '{workspacePath}'.", workspacePath);
        }

        try
        {
            var json = File.ReadAllText(workspacePath);
            using var doc = JsonDocument.Parse(json);
            if (!doc.RootElement.TryGetProperty("lastProjectPath", out var pathElement))
            {
                return VtprojLocatorResult.Failure("workspace.json missing lastProjectPath.", workspacePath);
            }

            var lastPath = pathElement.GetString();
            if (string.IsNullOrWhiteSpace(lastPath))
            {
                return VtprojLocatorResult.Failure("workspace.json lastProjectPath is empty.", workspacePath);
            }

            return VtprojLocatorResult.Success(lastPath, "workspace:lastProjectPath", workspacePath);
        }
        catch (Exception ex)
        {
            return VtprojLocatorResult.Failure($"Failed to parse workspace.json: {ex.Message}", workspacePath);
        }
    }

    private string? TryGetArgValue(string key)
    {
        for (var i = 0; i < _args.Length; i++)
        {
            var arg = _args[i];
            if (arg.StartsWith(key + "=", StringComparison.OrdinalIgnoreCase))
            {
                return arg.Substring(key.Length + 1).Trim('"');
            }

            if (string.Equals(arg, key, StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 < _args.Length)
                {
                    return _args[i + 1].Trim('"');
                }
            }
        }

        return null;
    }
}
