namespace Contracts;

public interface ILoggerManager
{
    public void LogDebug(string message, params object[]? args);
    public void LogInfo(string message, params object[]? args);
    public void LogWarning(string message, params object[]? args);
    public void LogError(string message, params object[]? args);
}