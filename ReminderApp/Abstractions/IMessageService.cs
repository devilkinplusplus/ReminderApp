﻿using ReminderApp.RequestParameters;
using Telegram.Bot.Types;

namespace ReminderApp.Abstractions
{
    public interface IMessageService
    {
        Task<Message> SendMessageAsync(string to, string content);
    }
}
