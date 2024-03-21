using DataLayer;
using Microsoft.Extensions.Logging;

namespace BusinessLogic;
/// <summary>
/// Представляет собой обработчик данных.
/// </summary>
public abstract class DataProcessing
{
    /// /// <summary>
    /// Конвертирует список объектов в потом с данными.
    /// </summary>
    /// <param name="data">Список объектов.</param>
    /// <returns>Поток с данными.</returns>
    public abstract Stream Write(List<Hockey> data);

    /// <summary>
    /// Конвертирует поток с данными в список объектов.
    /// </summary>
    /// <param name="stream">Потом с данными.</param>
    /// <returns>Список объектов.</returns>
    public abstract List<Hockey> Read(Stream stream);
}