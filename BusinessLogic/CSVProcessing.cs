using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataLayer;

namespace BusinessLogic;

public class CSVProcessing : FileProcessing
{
    private string[] _russianHeader = new string[]
    {
        "global_id", "Название спортивного объекта", "Название спортивной зоны в зимний период",
        "Фотография в зимний период", "Административный округ", "Район", "Адрес", "Адрес электронной почты",
        "Адрес сайта", "Справочный телефон", "Добавочный номер", "График работы в зимний период",
        "Уточнение графика работы в зимний период", "Вид собственности", "Тип ведомственной принадлежности",
        "Возможность проката оборудования", "Комментарии для проката оборудования",
        "Наличие сервиса технического обслуживания", "Комментарии для сервиса технического обслуживания",
        "Наличие раздевалки", "Наличие точки питания", "Наличие туалета", "Наличие точки Wi-Fi", "Наличие банкомата",
        "Наличие медпункта", "Наличие звукового сопровождения", "Период эксплуатации в зимний период",
        "Статус функционирования", "Размеры в зимний период", "Освещение", "Покрытие в зимний период",
        "Количество оборудованных посадочных мест", "Форма посещения (платность)", "Комментарии к стоимости посещения",
        "Приспособленность для занятий инвалидов", "Услуги предоставляемые в зимний период", "geoData",
        "geodata_center", "geoarea"
    };
    public override Stream Write(List<Hockey> data)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";",
            ShouldQuote = x => true
        };
        using (var stringWriter = new StringWriter())
        {
            using (var csv = new CsvWriter(stringWriter, config))
            {
                csv.WriteHeader<Hockey>();
                csv.NextRecord();
                foreach (string title in _russianHeader)
                {
                    csv.WriteField(title);
                }
                csv.NextRecord();
                csv.WriteRecords(data);
                csv.Flush();
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(stringWriter.ToString());
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
        }
    }

    public override List<Hockey> Read(Stream stream)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = s => s.Header.Trim('"'),
            Delimiter = ";",
            BadDataFound = null
        };
        StreamReader reader = new StreamReader(stream);
        using (var csv = new CsvReader(reader, config))
        {
            csv.Read();
            csv.ReadHeader();
            csv.Read();
            List<Hockey> data = csv.GetRecords<Hockey>().ToList();
            return data;
        }
    }
}