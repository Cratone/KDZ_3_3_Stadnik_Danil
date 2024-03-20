using System.Text;
using DataLayer;
using Newtonsoft.Json;

namespace BusinessLogic;

public class JSONProcessing : FileProcessing
{
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

    public override List<Hockey> Read(Stream stream)
    {
        using (var jsonTextReader = new JsonTextReader(new StreamReader(stream)))
        {
            var serializer = new JsonSerializer();
            return serializer.Deserialize<List<Hockey>>(jsonTextReader);
        }
    }
}