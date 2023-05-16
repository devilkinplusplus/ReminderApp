using Microsoft.EntityFrameworkCore;
using ReminderApp.Abstractions;
using ReminderApp.Consts;
using ReminderApp.Context;
using ReminderApp.Entities;
using ReminderApp.Enums;
using ReminderApp.RequestParameters;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ReminderApp.Concretes
{
    public class TelegramService : ITelegramService
    {
        private readonly ITodoService _todoService;
        public TelegramService(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public async Task<Message> SendMessageAsync(string to, string content)
        {
            await _todoService.AddAsync(new()
            {
                To = to,
                Content = content,
                SendAt = DateTime.Now,
                Method = MethodType.telegram.ToString(),
            });
            var botClient = new TelegramBotClient(TelegramApiInformations.ApiKey);
            return await botClient.SendTextMessageAsync(to, content);
        }

        public async Task UpdateMessageAsync(int messageId, string to, string content)
        {
            var botClient = new TelegramBotClient(TelegramApiInformations.ApiKey);
            await botClient.EditMessageTextAsync(to,messageId,content);
        }
    }
}
