using TodoApi.Dtos.TodoItemDtos;

namespace TodoApi.BusinessLogic.TodoItems
{
    public interface ITodoItemService
    {
        Task<TodoItemDto> CreateAsync(CreateTodoItem payload);
    }
}
