using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot;

public class TelegramInfoLogger
{
    private static TelegramInfoLogger s_instance = new TelegramInfoLogger();
    public static TelegramInfoLogger Instance
    {
        get => s_instance;
    }

    public void LogInfoGettingMessage(Message message, User user)
    {
        string text = $"Пользователь с Id {message.From.Id} и состоянием {user.State} отправил";
        if (message.Text is null || message.Text == "")
        {
            if (message.Document is not { } document)
            {
                text += " не текст и не файл.";
            }
            else
            {
                text += $" документ с названием {document.FileName}.";
            }
        }
        else
        {
            if (message.Document is not { } document)
            {
                text += $" сообщение \"{message.Text}\".";
            }
            else
            {
                text += $" сообщение {message.Text} и документ с названием {document.FileName}.";
            }
        }

        Program.MessageHandlerLogger.LogInformation(text);
    }

    public void LogInfoMenu(string menuLog, User user)
    {
        Program.MessageSenderLogger.LogInformation($"Пользователю с Id {user.Id} было показано {menuLog}.");
    }

    public void LogInfoSendingMessage(string text, User user)
    {
        Program.MessageSenderLogger.LogInformation($"Бот отправил пользователю с Id {user.Id} сообщение \"{text}\".");
    }

    public void LogInfoSendingFile(string fileName, User user)
    {
        Program.MessageSenderLogger.LogInformation($"Бот отправил пользователю с Id {user.Id} файл с названием {fileName}.");
    }

    public void LogInfoFileProcessing(string text, User user)
    {
        Program.FileProcessingLogger.LogInformation($"Данные пользователя с Id {user.Id} {text}.");
    }
}