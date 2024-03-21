using Microsoft.Extensions.Logging;

namespace BusinessLogic;

public static class FileLoggerExtensions
{
    /// <summary>
    /// Добавляет провайдер логирования в фабрику с указанным путем к файлу.
    /// </summary>
    /// <param name="factory">Фабрика.</param>
    /// <param name="filePath">Путь к файлу.</param>
    /// <returns>Фабрика с добавленным провайдером.</returns>
    public static ILoggerFactory AddFile(this ILoggerFactory factory, string filePath)
    {
        factory.AddProvider(new FileLoggerProvider(filePath));
        return factory;
    }
}