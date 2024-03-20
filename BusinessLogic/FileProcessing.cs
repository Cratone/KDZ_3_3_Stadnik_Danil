using DataLayer;
using Microsoft.Extensions.Logging;

namespace BusinessLogic;

public abstract class FileProcessing
{
    public ILogger Logger { get; set; }

    public abstract Stream Write(List<Hockey> data);

    public abstract List<Hockey> Read(Stream stream);
}