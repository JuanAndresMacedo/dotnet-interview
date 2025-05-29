using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos.TodoItemDtos;
using TodoApi.Models;

namespace TodoApi.BusinessLogic.TodoItems
{
    public class TodoItemService : ITodoItemService
    {
        private readonly TodoContext _context;

        public TodoItemService(TodoContext context)
        {
            _context = context;
        }

        public async Task<TodoItemDto?> CreateAsync(long todoListId, CreateTodoItem payload)
        {
            var todoList = await _context.TodoList.FindAsync(todoListId);

            if (todoList == null)
                return null;

            var todoItem = new TodoItem
            {
                Description = payload.Description,
                TodoListId = todoListId
            };

            _context.TodoItem.Add(todoItem);
            await _context.SaveChangesAsync();

            return new TodoItemDto
            {
                Id = todoItem.Id,
                Description = todoItem.Description,
                IsCompleted = todoItem.IsCompleted
            };
        }

        public async Task<TodoItemDto?> UpdateDescriptionAsync(long todoListId, long todoItemId,
            UpdateTodoItem payload)
        {
            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(i => i.Id == todoItemId && i.TodoListId == todoListId);

            if (todoItem == null)
                return null;

            todoItem.Description = payload.Description;
            await _context.SaveChangesAsync();

            return new TodoItemDto
            {
                Id = todoItem.Id,
                Description = todoItem.Description,
                IsCompleted = todoItem.IsCompleted
            };
        }

        public async Task<bool> CompleteAsync(long todoListId, long todoItemId)
        {
            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(i => i.Id == todoItemId && i.TodoListId == todoListId);

            if (todoItem == null)
                return false;

            todoItem.IsCompleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(long todoListId, long todoItemId)
        {
            var todoItem = await _context.TodoItem
                .FirstOrDefaultAsync(i => i.Id == todoItemId && i.TodoListId == todoListId);

            if (todoItem == null)
                return false;

            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
