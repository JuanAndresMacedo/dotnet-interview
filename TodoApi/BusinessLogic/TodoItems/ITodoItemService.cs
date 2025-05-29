using TodoApi.Dtos.TodoItemDtos;

namespace TodoApi.BusinessLogic.TodoItems
{
    public interface ITodoItemService
    {
        Task<TodoItemDto?> CreateAsync(long todoListId, CreateTodoItem payload);
        Task<TodoItemDto?> UpdateDescriptionAsync(long todoListId,
            long itemId, UpdateTodoItem payload);
        Task<bool> CompleteAsync(long todoListId, long itemId);
    }
}
