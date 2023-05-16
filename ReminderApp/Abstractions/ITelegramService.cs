namespace ReminderApp.Abstractions
{
    public interface ITelegramService : IMessageService
    {
        Task UpdateMessageAsync(int messageId, string to, string content);
    }
}
