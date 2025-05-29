using TodoApi.Dtos.TodoItemDtos;
using TodoApi.Dtos.TodoListDtos;
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

        public Task<TodoItemDto> CreateAsync(CreateTodoItem payload)
        {
            TodoList todoList = new TodoList
            {
                Name = payload.Name
            };

            _context.TodoItem.Add(todoList);
            await _context.SaveChangesAsync();

            return new TodoListDto
            {
                Id = todoList.Id,
                Name = todoList.Name
            };
        }
    }
}
