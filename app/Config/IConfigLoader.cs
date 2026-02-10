namespace app.Config;

public interface IConfigLoader
{
    VtprojProjectContext LoadProject();
    string ResolveAssetPath(string rawPath, string source);
}
