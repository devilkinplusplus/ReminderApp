using ReminderApp.Entities;

namespace ReminderApp.Abstractions
{
    public interface ITodoService
    {
        Task AddAsync(Todo todo);
        Task UpdateAsync(Todo todo);
        Task DeleteAsync(Todo todo);
        Task<IEnumerable<Todo>> GetAllAsync();
        Task<Todo> GetAsync(int id);
    }
}
