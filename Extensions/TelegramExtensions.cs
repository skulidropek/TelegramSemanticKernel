using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramSemanticKernel.Services;

namespace TelegramSemanticKernel.Extensions
{
    public static class TelegramExtensions
    {
        public static void AddTelegram(this IServiceCollection services, string token)
        {
            services.AddSingleton<ITelegramBotClient>(s => new TelegramBotClient(token));
            services.AddSingleton<TelegramBotService>();
        }

        public static async Task<string> SendStreamingChatMessageContentsAsync(this ITelegramBotClient client, Chat chat, IAsyncEnumerable<StreamingChatMessageContent> result, string firstMessage = "Generate....", CancellationToken cancellationToken = default)
        {
            var message = await client.SendTextMessageAsync(chat, firstMessage, cancellationToken: cancellationToken);

            await client.SendChatActionAsync(chat, ChatAction.Typing, cancellationToken: cancellationToken);

            var fullMessage = "";
            var oldMessage = "";
            var cellMssage = "";

            await foreach (var content in result)
            {
                cellMssage += content.Content;

                if (cellMssage.Length > 150)
                {
                    fullMessage += cellMssage;
                    oldMessage = fullMessage;
                    cellMssage = "";
                    message = await client.EditMessageTextAsync(chat, message.MessageId, fullMessage, cancellationToken: cancellationToken);
                    await client.SendChatActionAsync(chat, ChatAction.Typing, cancellationToken: cancellationToken);
                }
            }

            if (cellMssage.Length > 0)
            {
                fullMessage += cellMssage;
            }

            if (oldMessage != fullMessage)
            {
                await client.EditMessageTextAsync(chat, message.MessageId, fullMessage, cancellationToken: cancellationToken);
            }

            return fullMessage;
        }
    }
}
