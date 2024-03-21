using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using DataLayer;

namespace BusinessLogic;
/// <summary>
/// Предаствляет собой обработчик csv-данных.
/// </summary>
public class CSVProcessing : DataProcessing
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
    /// <summary>
    /// Конвертирует список объектов в потом с csv-данными.
    /// </summary>
    /// <param name="data">Список объектов.</param>
    /// <returns>Поток с csv-данными.</returns>
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
                // Добавляет русский заголовок.
                foreach (string title in _russianHeader)
                {
                    csv.WriteField(title);
                }
                csv.NextRecord();
                csv.WriteRecords(data);
                csv.Flush();
                // Преобразует csv в поток.
                var stream = new MemoryStream();
                var writer = new StreamWriter(stream);
                writer.Write(stringWriter.ToString());
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
        }
    }

    /// <summary>
    /// Конвертирует поток с cvs-данными в список объектов.
    /// </summary>
    /// <param name="stream">Потом с csv-данными.</param>
    /// <returns>Список объектов.</returns>
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
            // Считывает английский заголовок.
            csv.Read();
            csv.ReadHeader();
            // Считывает русский заголовок.
            csv.Read();
            List<Hockey> data = csv.GetRecords<Hockey>().ToList();
            return data;
        }
    }
}