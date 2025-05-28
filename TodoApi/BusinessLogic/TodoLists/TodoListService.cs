using TodoApi.Models;
using TodoApi.Dtos;
using Microsoft.EntityFrameworkCore;

namespace TodoApi.BusinessLogic.TodoLists
{
    public class TodoListService : ITodoListService
    {
        private readonly TodoContext _context;

        public TodoListService(TodoContext context)
        {
            _context = context;
        }

        public async Task<TodoListDto> CreateAsync(CreateTodoList payload)
        {
            TodoList todoList = new TodoList
            {
                Name = payload.Name
            };

            _context.TodoList.Add(todoList);
            await _context.SaveChangesAsync();

            return new TodoListDto
            {
                Id = todoList.Id,
                Name = todoList.Name
            };
        }

        public async Task<IList<TodoListDto>> GetAllAsync()
        {
            return await _context.TodoList.AsNoTracking()
                .Select(t => new TodoListDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToListAsync();
        }

        public async Task<TodoListDto?> GetByIdAsync(long id)
        {
            var todoList = await _context.TodoList
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == id);

            if (todoList == null)
                return null;

            return new TodoListDto
            {
                Id = todoList.Id,
                Name = todoList.Name
            };
        }

        public async Task<TodoListDto?> UpdateAsync(long id, UpdateTodoList payload)
        {
            var todoList = await _context.TodoList.FindAsync(id);

            if (todoList == null)
                return null;

            todoList.Name = payload.Name;
            await _context.SaveChangesAsync();

            return new TodoListDto
            {
                Id = todoList.Id,
                Name = todoList.Name
            };
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var todoList = await _context.TodoList.FindAsync(id);
            
            if (todoList == null)
                return false;

            _context.TodoList.Remove(todoList);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
