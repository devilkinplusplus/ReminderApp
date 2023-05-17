using ReminderApp.Entities;
using ReminderApp.Enums;
using ReminderApp.RequestParameters;

namespace ReminderApp.Abstractions
{
    public interface IOperations
    {
        void SendMessageAtTime(string to, string content, DateTime scheduledDate, MethodType method);
        Task UpdateMessageAsync(int id, string to, string content, DateTime sendAt, MethodType method);
        Task DeleteAsync(int id);
        Task<IEnumerable<Todo>> GetAllTodosAsync();
    }
}
