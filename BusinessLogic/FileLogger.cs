using Microsoft.Extensions.Logging;

namespace BusinessLogic;

public class FileLogger : ILogger
{
    private readonly string _filePath;
    private static readonly object S_Lock = new();
    public FileLogger(string path)
    {
        _filePath = path;
    }
    public IDisposable? BeginScope<TState>(TState state)
    {
        return null;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception, string>? formatter)
    {
        if (formatter == null) return;

        var exc = "";
        lock (S_Lock)
        {
            var fullFilePath = Path.Combine(_filePath, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
            var n = Environment.NewLine;
            if (exception != null)
                exc = n + exception.GetType() + ": " + exception.Message + n + exception.StackTrace + n;
            File.AppendAllText(fullFilePath, $"{DateTime.Now:T} {logLevel}: {formatter(state, exception)}{n}{exc}");
        }
    }
}