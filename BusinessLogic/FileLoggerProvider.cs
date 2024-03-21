using Microsoft.Extensions.Logging;

namespace BusinessLogic;
/// <summary>
/// Представляет собой тип, который может создать файловые логгеры.
/// </summary>
public class FileLoggerProvider : ILoggerProvider
{
    private readonly string _path;
    public FileLoggerProvider(string path)
    {
        _path = path;
    }

    /// <summary>
    /// Создает новый файловый логгер.
    /// </summary>
    /// <param name="categoryName">Имя категории.</param>
    /// <returns>Созданный логгер.</returns>
    public ILogger CreateLogger(string categoryName)
        => new FileLogger(_path);


    public void Dispose() { }
}