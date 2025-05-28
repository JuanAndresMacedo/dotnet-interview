using TodoApi.Models;
using TodoApi.Dtos;

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
    }
}
