using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using System.Threading;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.TextToImage;

namespace TelegramSemanticKernel.Services
{
    public class TelegramBotService(ITelegramBotClient telegramBotClient, IChatCompletionService chatService, ILogger<TelegramBotService> logger)
    {
        //if (update.Message.Document != null)
        //{
        //    var file = await telegramBotClient.GetFileAsync(update.Message.Document.FileId, cancellationToken: cancellationToken);

        //    string saveFilePath = $"{update.Message.Chat.Id}/{update.Message.Document.FileName}";

        //    if (!Directory.Exists(update.Message.Chat.Id.ToString()))
        //    {
        //        Directory.CreateDirectory(update.Message.Chat.Id.ToString());
        //    }

        //    using (var saveFileStream = System.IO.File.Create(saveFilePath))
        //    {
        //        // Скачивание файла
        //        await telegramBotClient.DownloadFileAsync(file.FilePath, saveFileStream, cancellationToken: cancellationToken);

        //        string fileContent = await System.IO.File.ReadAllTextAsync(saveFilePath, cancellationToken);
        //        chatHistory.AddUserMessage(fileContent);
        //    }
        //}

        public void ChatStart(ReceiverOptions receiverOptions, Func<ITelegramBotClient, Update, CancellationToken, Task> handleUpdateAsync, CancellationToken cancellationToken = default)//
        {

            telegramBotClient.StartReceiving(
                handleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken
            );  
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError($"An error occurred {exception.Message}", exception);
            return Task.CompletedTask;
        }
    }
}
