using Microsoft.EntityFrameworkCore;
using ReminderApp.Abstractions;
using ReminderApp.Context;
using ReminderApp.Entities;

namespace ReminderApp.Concretes
{
    public class TodoService : ITodoService
    {
        // Don't need to implement repository design pattern
        private readonly AppDbContext _context;
        public TodoService(AppDbContext context) => _context = context;

        public async Task AddAsync(Todo todo)
        {
            await _context.Todos.AddAsync(todo);
            await SaveAsync();
        }

        public async Task DeleteAsync(Todo todo)
        {
            _context.Todos.Remove(todo);
            await SaveAsync();
        }

        public async Task<IEnumerable<Todo>> GetAllAsync()
        {
            return await _context.Todos.ToListAsync();
        }

        public async Task<Todo> GetAsync(int id)
        {
            return await _context.Todos.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Todo todo)
        {
            _context.Todos.Update(todo);
            await SaveAsync();
        }

        private async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}
