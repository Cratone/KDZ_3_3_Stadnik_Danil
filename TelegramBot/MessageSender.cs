using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace TelegramBot;

public class MessageSender
{
    public User User { get; set; }

    private ReplyKeyboardMarkup _mainMenuKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new[]
        {
            new KeyboardButton("Загрузить новый файл"),
            new KeyboardButton("Взаимодействие с файлом")
        }
    })
    {
        ResizeKeyboard = true
    };

    private ReplyKeyboardMarkup _actionWithFileKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new[]
        {
            new KeyboardButton("Отфильтровать данные"),
            new KeyboardButton("Отсортировать данные"),
            new KeyboardButton("Скачать файл"),
            new KeyboardButton("Удалить файл")
        }
    })
    {
        ResizeKeyboard = true
    };

    private ReplyKeyboardMarkup _fieldsForFilterKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new[]
        {
            new KeyboardButton("ObjectName"),
            new KeyboardButton("NameWinter"),
            new KeyboardButton("District и HasDressingRoom")
        }
    })
    {
        ResizeKeyboard = true
    };

    private ReplyKeyboardMarkup _fieldsForSortKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new[]
        {
            new KeyboardButton("Lighting по алфавиту в прямом порядке"),
            new KeyboardButton("Seats в порядке возрастания")
        }
    })
    {
        ResizeKeyboard = true
    };

    private ReplyKeyboardMarkup _extensionKeyboard = new ReplyKeyboardMarkup(new[]
    {
        new[]
        {
            new KeyboardButton("CSV"),
            new KeyboardButton("JSON")
        }
    })
    {
        ResizeKeyboard = true
    };

    public async Task ShowKeyboardAsync(ITelegramBotClient botClient)
    {
        switch (User.State)
        {
            case UserState.MainMenu:
                await botClient.SendTextMessageAsync(User.ChatId, "Выберите пункт меню", replyMarkup: _mainMenuKeyboard);
                TelegramInfoLogger.Instance.LogInfoMenu("главное меню", User);
                break;
            case UserState.ActionWithFile:
                await botClient.SendTextMessageAsync(User.ChatId, "Что нужно сделать с файлом?", replyMarkup: _actionWithFileKeyboard);
                TelegramInfoLogger.Instance.LogInfoMenu("меню с выбором действия над файлом", User);
                break;
            case UserState.SelectExtensionForDownloading:
                await botClient.SendTextMessageAsync(User.ChatId, "Выберите расширение файла", replyMarkup: _extensionKeyboard);
                TelegramInfoLogger.Instance.LogInfoMenu("меню с выбором расширения файла для скачивания", User);
                break;
            case UserState.SelectFieldsForFilter:
                await botClient.SendTextMessageAsync(User.ChatId, "Выберите поля для фильтрации", replyMarkup: _fieldsForFilterKeyboard);
                TelegramInfoLogger.Instance.LogInfoMenu("меню с выбором полей для фильтрации", User);
                break;
            case UserState.SelectFieldForSort:
                await botClient.SendTextMessageAsync(User.ChatId, "Выберите поле для сортировки", replyMarkup: _fieldsForSortKeyboard);
                TelegramInfoLogger.Instance.LogInfoMenu("меню с выбором поля для сортировки", User);
                break;
            default:
                return;
        }
    }

    public async Task SendMessageAsync(ITelegramBotClient botClient, string text)
    {
        await botClient.SendTextMessageAsync(User.ChatId, text, replyMarkup: new ReplyKeyboardRemove());
        TelegramInfoLogger.Instance.LogInfoSendingMessage(text, User);
    }

    public async Task SendFileFromStreamAsync(ITelegramBotClient botClient, Stream stream, string fileName)
    {
        await botClient.SendDocumentAsync(
            chatId: User.ChatId,
            document: InputFile.FromStream(
                stream: stream,
                fileName: fileName)
        );
        TelegramInfoLogger.Instance.LogInfoSendingFile(fileName, User);
    }

    public async Task SendStartMessageAsync(ITelegramBotClient botClient)
    {
        string text =
            "Привет. Я бот, который поможет тебе с обработкой csv и json файлов. В них должна храниться информация о " +
            "катках. Можешь посмотреть примеры файлов с помощью команды /examples. Важно, что при сохранении в системе " +
            "двух файлов с одинаковым названием (расширение не имеет значение) более старый файл будет утерян.";
        SendMessageAsync(botClient, text);
    }

    public async Task SendExampleFilesAsync(ITelegramBotClient botClient)
    {
        string path = Path.Combine("..", "..", "..", "..", "Documentation");
        await SendFileFromStreamAsync(
            botClient, File.OpenRead(Path.Combine(path, "Example.csv")), "Example.csv"
            );
        await SendFileFromStreamAsync(
            botClient, File.OpenRead(Path.Combine(path, "Example.json")), "Example.json"
            );
    }

    public MessageSender() { }

    public MessageSender(User user)
    {
        User = user;
    }
}