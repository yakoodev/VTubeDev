namespace app.Config;

public sealed class ConfigPathException : Exception
{
    public ConfigPathException(string message) : base(message)
    {
    }
}
