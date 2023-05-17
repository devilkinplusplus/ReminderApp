using Hangfire;
using Hangfire.States;
using Microsoft.EntityFrameworkCore;
using ReminderApp.Abstractions;
using ReminderApp.Context;
using ReminderApp.Entities;
using ReminderApp.Enums;
using ReminderApp.RequestParameters;

namespace ReminderApp.Concretes
{
    public class Operations : IOperations
    {
        private readonly ITodoService _todoService;
        public Operations(ITodoService todoService)
        {
            _todoService = todoService;
        }

        public void SendMessageAtTime(string to, string content, DateTime scheduledDate, MethodType method)
        {
            var delay = scheduledDate - DateTime.UtcNow;

            if (!IsValidDateTime(scheduledDate))
                throw new Exception("Invalid send time");

            switch (method)
            {
                case MethodType.Telegram:
                    BackgroundJob.Schedule<TelegramService>(x => x.SendMessageAsync(to, content), delay);
                    break;
                case MethodType.Email:
                    BackgroundJob.Schedule<MailService>(x => x.SendMessageAsync(to, content), delay);
                    break;
                default:
                    throw new Exception("No method found");
            }
        }
        public async Task<IEnumerable<Todo>> GetAllTodosAsync() => await _todoService.GetAllAsync();
        public async Task DeleteAsync(int id)
        {
            Todo? entity = await _todoService.GetAsync(id);
            await _todoService.DeleteAsync(entity);
        }
        public async Task UpdateMessageAsync(int id, string to, string content, DateTime sendAt, MethodType method)
        {
            try
            {
                Todo todo = await _todoService.GetAsync(id);
                todo.To = to ?? todo.To;
                todo.Content = content ?? todo.Content;
                todo.SendAt = sendAt;
                todo.Method = method.ToString() ?? todo.Method;
                await _todoService.UpdateAsync(todo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private bool IsValidDateTime(DateTime sendAt)
        {
            DateTime currentDateTime = DateTime.UtcNow;
            return sendAt > currentDateTime;
        }
    }
}
