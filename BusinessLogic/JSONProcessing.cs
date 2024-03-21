using System.Text;
using DataLayer;
using Newtonsoft.Json;

namespace BusinessLogic;
/// <summary>
/// Предаствляет собой обработчик json-данных.
/// </summary>
public class JSONProcessing : DataProcessing
{
    /// <summary>
    /// Конвертирует список объектов в потом с json-данными.
    /// </summary>
    /// <param name="data">Список объектов.</param>
    /// <returns>Поток с json-данными.</returns>
    public override Stream Write(List<Hockey> data)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented
        };
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        var serializer = JsonSerializer.Create(settings);
        serializer.Serialize(writer, data);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
    
    /// <summary>
    /// Конвертирует поток с json-данными в список объектов.
    /// </summary>
    /// <param name="stream">Потом с json-данными.</param>
    /// <returns>Список объектов.</returns>
    public override List<Hockey> Read(Stream stream)
    {
        using (var jsonTextReader = new JsonTextReader(new StreamReader(stream)))
        {
            var serializer = new JsonSerializer();
            return serializer.Deserialize<List<Hockey>>(jsonTextReader);
        }
    }
}