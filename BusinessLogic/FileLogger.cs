using Microsoft.Extensions.Logging;

namespace BusinessLogic;
/// <summary>
/// Представляет собой логгер информации в файл.
/// </summary>
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

    /// <summary>
    /// Логирует информацию в файл.
    /// </summary>
    /// <param name="logLevel">Уровень лога.</param>
    /// <param name="eventId">Id события.</param>
    /// <param name="state">Состояние.</param>
    /// <param name="exception">Исключение.</param>
    /// <param name="formatter">Конвертер в нужный формат.</param>
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