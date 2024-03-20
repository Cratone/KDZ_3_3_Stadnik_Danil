using BusinessLogic;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace TelegramBot
{
    internal static class Program
    {
        public static ILogger MessageHandlerLogger { get; set; }
        public static ILogger MessageSenderLogger { get; set; }
        public static ILogger FileProcessingLogger { get; set; }
        
        public static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient("6997927085:AAElpTX2zaIJ4KOhf-w3VMnG-tS7vpLj2xQ");
            using CancellationTokenSource cts = new();
            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };
            
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                // Добавление провайдера логирования в файл с настройками
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                });
            });

            loggerFactory.AddFile(Path.Combine("..", "..", "..", "..", "var"));
            MessageHandler messageHandler = new MessageHandler();
            MessageHandlerLogger = loggerFactory.CreateLogger<MessageHandler>();
            MessageSenderLogger = loggerFactory.CreateLogger<MessageSender>();
            FileProcessingLogger = loggerFactory.CreateLogger<FileProcessing>();
            botClient.StartReceiving(
                updateHandler: messageHandler.HandleUpdateAsync,
                pollingErrorHandler: messageHandler.HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }
    }
}