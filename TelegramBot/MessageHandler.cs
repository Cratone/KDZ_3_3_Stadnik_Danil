using BusinessLogic;
using DataLayer;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot;

public class MessageHandler
{
    public Dictionary<long, User> Users = new Dictionary<long, User>();

    public Dictionary<UserState, Func<ITelegramBotClient, Message, CancellationToken, User, Task>> Handlers { get; set; }
    
    public MessageHandler()
    {
        Handlers = new Dictionary<UserState, Func<ITelegramBotClient, Message, CancellationToken, User, Task>>
        {
            { UserState.Start , HandleStartMessageAsync},
            { UserState.MainMenu , HandleMainMenuMessageAsync},
            { UserState.SendingFile, HandleSendingFileMessageAsync},
            { UserState.SelectFile , HandleSelectFileMessageAsync},
            { UserState.ActionWithFile , HandleActionWithFileMessageAsync},
            { UserState.SelectFieldsForFilter , HandleSelectFieldsForFilterMessageAsync},
            { UserState.SelectValuesForFilter , HandleSelectValuesForFilterMessageAsync},
            { UserState.SelectFieldForSort , HandleSelectFieldForSortMessageAsync},
            { UserState.SelectNameForSaving , HandleSelectNameForSavingMessageAsync},
            { UserState.SelectExtensionForDownloading , HandleSelectExtensionForDownloadingMessageAsync}
        };
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;

        long chatId = message.Chat.Id;
        Console.WriteLine(chatId);
        if (!Users.ContainsKey(chatId))
        {
            Users[chatId] = new User(chatId, message.From.Id);
            // TODO Переделать
            Users[chatId].Sender.SendMessageAsync(botClient, "Привет");
        }
        User user = Users[chatId];
        TelegramInfoLogger.LogInfoGettingMessage(message, user);
        if (message.Text == "/start")
        {
            user.State = UserState.MainMenu;
            await user.Sender.ShowKeyboardAsync(botClient);
            return;
        }

        if (message.Text == "/help")
        {
            // TODO Переделать
            await user.Sender.SendMessageAsync(botClient, "Ну всё же и так понятно. Сам разберешься, я в тебя верю!");
        }
        Handlers[Users[chatId].State]?.Invoke(botClient, message, cancellationToken, user);
    }

    public async Task HandleStartMessageAsync(ITelegramBotClient botClient, Message message, 
        CancellationToken cancellationToken, User user)
    {
        user.State = UserState.MainMenu;
        user.Sender.ShowKeyboardAsync(botClient);
    }

    public async Task HandleMainMenuMessageAsync(ITelegramBotClient botClient, Message message, 
        CancellationToken cancellationToken, User user)
    {
        switch (message.Text)
        {
            case "Загрузить новый файл":
                user.State = UserState.SendingFile;
                await user.Sender.SendMessageAsync(botClient, "Пришлите файл с расширением csv или json");
                break;
            case "Взаимодейтсвие с файлом":
                if (user.Files.Count == 0)
                {
                    await user.Sender.SendMessageAsync(botClient, "У вас нет загруженных файлов, чтобы работать с ними");
                    await user.Sender.ShowKeyboardAsync(botClient);
                    break;
                }
                user.State = UserState.SelectFile;
                string text = "Напишите имя нужного файла. Вот список имен доступных вам файлов:\n" + 
                              string.Join("\n", user.Files.Keys.ToList());
                await user.Sender.SendMessageAsync(botClient, text);
                break;
            default:
                return;
        }
    }

    public async Task HandleSendingFileMessageAsync(ITelegramBotClient botClient, Message message, 
        CancellationToken cancellationToken, User user)
    {
        if (message.Document is not { } document)
            return;
        FileProcessing proceccor;
        string fileName;
        switch (document.FileName.Split(".")[^1])
        {
            case "csv":
                proceccor = new CSVProcessing();
                fileName = document.FileName[..^4];
                break;
            case "json":
                proceccor = new JSONProcessing();
                fileName = document.FileName[..^5];
                break;
            default:
                return;
        }
        var fileId = document.FileId;
        string destinationFilePath = $"{user.ChatId}.file";
        await using Stream fileStream = System.IO.File.Create(destinationFilePath);
        await botClient.GetInfoAndDownloadFileAsync(
            fileId: fileId,
            destination: fileStream,
            cancellationToken: cancellationToken);
        fileStream.Close();
        FileStream stream = System.IO.File.OpenRead(destinationFilePath);
        try
        {
            List<Hockey> data = proceccor.Read(stream);
            user.Files[fileName] = data;
            await user.Sender.SendMessageAsync(botClient, $"Файл {document.FileName} успешно сохранен");
            user.State = UserState.MainMenu;
            await user.Sender.ShowKeyboardAsync(botClient);
            TelegramInfoLogger.LogInfoFileProcessing("были успешно обработаны", user);
        }
        catch (CsvHelper.CsvHelperException)
        {
            await user.Sender.SendMessageAsync(botClient, "Данные в файле представлены в неправильном формате");
            TelegramInfoLogger.LogInfoFileProcessing("не были успешно обработаны", user);
        }
        catch (Newtonsoft.Json.JsonException)
        {
            await user.Sender.SendMessageAsync(botClient, "Данные в файле представлены в неправильном формате");
            TelegramInfoLogger.LogInfoFileProcessing("не были успешно обработаны", user);
        }
    }

    public async Task HandleSelectFileMessageAsync(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken, User user)
    {
        if (user.Files.Keys.Contains(message.Text))
        {
            user.SelectedFileName = message.Text;
            user.SelectedFile = user.Files[user.SelectedFileName];
            user.State = UserState.ActionWithFile;
            await user.Sender.ShowKeyboardAsync(botClient);
        }
        else
        {
            await user.Sender.SendMessageAsync(botClient, "Файла с таким именем нет");
        }
    }

    public async Task HandleActionWithFileMessageAsync(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken, User user)
    {
        switch (message.Text)
        {
            case "Отфильтровать данные":
                user.State = UserState.SelectFieldsForFilter;
                await user.Sender.ShowKeyboardAsync(botClient);
                break;
            case "Отсортировать данные":
                user.State = UserState.SelectFieldForSort;
                await user.Sender.ShowKeyboardAsync(botClient);
                break;
            case "Скачать файл":
                user.State = UserState.SelectExtensionForDownloading;
                await user.Sender.ShowKeyboardAsync(botClient);
                break;
            case "Удалить файл":
                user.Files.Remove(user.SelectedFileName);
                await user.Sender.SendMessageAsync(botClient, "Файл удален");
                user.State = UserState.MainMenu;
                await user.Sender.ShowKeyboardAsync(botClient);
                break;
            default:
                return;
        }
    }

    public async Task HandleSelectFieldsForFilterMessageAsync(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken, User user)
    {
        switch (message.Text)
        {
            case "ObjectName":
                user.State = UserState.SelectValuesForFilter;
                user.SelectedFields = new List<string>() { "ObjectName" };
                user.SelectedValues = new List<string>();
                await user.Sender.SendMessageAsync(botClient, $"Введите нужное значение для поля {user.SelectedFields[0]}");
                break;
            case "NameWinter":
                user.State = UserState.SelectValuesForFilter;
                user.SelectedFields = new List<string>() { "NameWinter" };
                user.SelectedValues = new List<string>();
                await user.Sender.SendMessageAsync(botClient, $"Введите нужное значение для поля {user.SelectedFields[0]}");
                break;
            case "District и HasDressingRoom":
                user.State = UserState.SelectValuesForFilter;
                user.SelectedFields = new List<string>() { "District", "HasDressing" };
                user.SelectedValues = new List<string>();
                await user.Sender.SendMessageAsync(botClient, $"Введите нужное значение для поля {user.SelectedFields[0]}");
                break;
            default:
                return;
        }
    }

    public async Task HandleSelectValuesForFilterMessageAsync(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken, User user)
    {
        if (message.Text is null || message.Text == "")
        {
            return;
        }

        user.SelectedValues.Add(message.Text);
        int n = user.SelectedFields.Count;
        if (n != user.SelectedFields.Count)
        {
            await user.Sender.SendMessageAsync(botClient, $"Введите нужное значение для поля {user.SelectedFields[n]}");
            return;
        }

        for (int i = 0; i < user.SelectedFields.Count; i++)
        {
            user.SelectedFile = user.SelectedFile.Where(hockey => hockey[user.SelectedFields[i]] == user.SelectedValues[i]).ToList();
        }

        await user.Sender.SendMessageAsync(botClient, $"Данные отфильтрованны, объектов {user.SelectedFile.Count}");
        user.State = UserState.SelectNameForSaving;
        await user.Sender.SendMessageAsync(botClient, "Введите имя файла, в который нужно сохранить полученные данные");
    }

    public async Task HandleSelectFieldForSortMessageAsync(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken, User user)
    {
        switch (message.Text)
        {
            case "Lighting по алфавиту в прямом порядке":
                user.SelectedFile = user.SelectedFile.OrderBy(hockey => hockey.Lighting).ToList();
                await user.Sender.SendMessageAsync(botClient, $"Данные отсортированы");
                user.State = UserState.SelectNameForSaving;
                await user.Sender.SendMessageAsync(botClient, "Введите имя файла, в который нужно сохранить полученные данные");
                break;
            case "Seats в порядке возрастания":
                user.SelectedFile = user.SelectedFile.OrderBy(hockey => hockey.Seats).ToList();
                await user.Sender.SendMessageAsync(botClient, $"Данные отсортированы");
                user.State = UserState.SelectNameForSaving;
                await user.Sender.SendMessageAsync(botClient, "Введите имя файла без расширения, в который нужно сохранить полученные данные");
                break;
            default:
                return;
        }
    }

    public async Task HandleSelectNameForSavingMessageAsync(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken, User user)
    {
        if (message.Text is null || message.Text == "")
        {
            return;
        }

        user.Files[message.Text] = user.SelectedFile;
        await user.Sender.SendMessageAsync(botClient, "Данные сохранены");
        user.State = UserState.MainMenu;
        await user.Sender.ShowKeyboardAsync(botClient);
    }

    public async Task HandleSelectExtensionForDownloadingMessageAsync(ITelegramBotClient botClient, Message message,
        CancellationToken cancellationToken, User user)
    {
        FileProcessing processor;
        switch (message.Text)
        {
            case "CSV":
                processor = new CSVProcessing();
                break;
            case "JSON":
                processor = new JSONProcessing();
                break;
            default:
                return;
        }

        await user.Sender.SendFileFromStreamAsync(botClient, processor.Write(user.SelectedFile), 
            user.SelectedFileName + "." + message.Text.ToLower());
        user.State = UserState.MainMenu;
        await user.Sender.ShowKeyboardAsync(botClient);
    }

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}