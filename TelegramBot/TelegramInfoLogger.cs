using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBot;
/// <summary>
/// Представляет собой логгер информации о работе телеграмм-бота.
/// </summary>
public class TelegramInfoLogger
{
    private static TelegramInfoLogger s_instance = new TelegramInfoLogger();
    public static TelegramInfoLogger Instance
    {
        get => s_instance;
    }

    /// <summary>
    /// Логирует информацию о полученном сообщении.
    /// </summary>
    /// <param name="message">Полученное сообщение</param>
    /// <param name="user">Пользователь, отправивший сообщение.</param>
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

    /// <summary>
    /// Логирует информацию о выведенном меню.
    /// </summary>
    /// <param name="menuLog">Описание выведенного меню.</param>
    /// <param name="user">Пользователь, которому вывели меню.</param>
    public void LogInfoMenu(string menuLog, User user)
    {
        Program.MessageSenderLogger.LogInformation($"Пользователю с Id {user.Id} было показано {menuLog}.");
    }

    /// <summary>
    /// Логирует информацию об отправленном сообщении.
    /// </summary>
    /// <param name="text">Текст отправленногосообщения.</param>
    /// <param name="user">Пользователь, которому отправилось сообщение.</param>
    public void LogInfoSendingMessage(string text, User user)
    {
        Program.MessageSenderLogger.LogInformation($"Бот отправил пользователю с Id {user.Id} сообщение \"{text}\".");
    }

    /// <summary>
    /// Логирует информвцию об отправленнос файле.
    /// </summary>
    /// <param name="fileName">Имя отправленного файла.</param>
    /// <param name="user">Пользователь, которому отправили файл.</param>
    public void LogInfoSendingFile(string fileName, User user)
    {
        Program.MessageSenderLogger.LogInformation($"Бот отправил пользователю с Id {user.Id} файл с названием {fileName}.");
    }

    /// <summary>
    /// Логирует информацию об обработке данных.
    /// </summary>
    /// <param name="text">Информацию о результате обработки.</param>
    /// <param name="user">Пользователь, чьи данные обрабатывались.</param>
    public void LogInfoProcessing(string text, User user)
    {
        Program.FileProcessingLogger.LogInformation($"Данные пользователя с Id {user.Id} {text}.");
    }

    /// <summary>
    /// Логирует информацию об ошибке.
    /// </summary>
    /// <param name="errorMessage">Текст ошибки.</param>
    public void LogError(string errorMessage)
    {
        Program.MessageHandlerLogger.LogError(errorMessage);
    }
}