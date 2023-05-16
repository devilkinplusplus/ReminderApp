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
        private readonly AppDbContext _context;
        private readonly ITodoService _todoService;
        public Operations(AppDbContext context, ITodoService todoService)
        {
            _context = context;
            _todoService = todoService;
        }

        public void SendMessageAtTime(string to, string content, DateTime scheduledDate, MethodType method)
        {
            var delay = scheduledDate - DateTime.UtcNow;

            if (!IsValidDateTime(scheduledDate))
                throw new Exception("Invalid send time");

            switch (method)
            {
                case MethodType.telegram:
                    BackgroundJob.Schedule<TelegramService>(x => x.SendMessageAsync(to, content), delay);
                    break;
                case MethodType.email:
                    BackgroundJob.Schedule<MailService>(x => x.SendMessageAsync(to, content), delay);
                    break;
                default:
                    throw new Exception("No method found");
            }
        }


        public async Task UpdateMessageAsync(int id, string to, string content, DateTime sendAt, MethodType method)
        {
            var delay = sendAt - DateTime.UtcNow;

            if (!IsValidDateTime(sendAt))
                throw new Exception("Invalid send time");

            switch (method)
            {
                case MethodType.telegram:
                    BackgroundJob.Schedule<TelegramService>(x => x.UpdateMessageAsync(id,to,content), delay);
                    break;
                case MethodType.email:
                    BackgroundJob.Schedule<MailService>(x => x.SendMessageAsync(to, content), delay);
                    break;
                default:
                    throw new Exception("No method found");
            }
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync() => await _context.Todos.ToListAsync();
        public async Task DeleteAsync(int id)
        {
            Todo? entity = await _context.Todos.FindAsync(id);
            await _todoService.DeleteAsync(entity);
        }
        private bool IsValidDateTime(DateTime sendAt)
        {
            DateTime currentDateTime = DateTime.UtcNow;
            return sendAt > currentDateTime;
        }

    }
}
