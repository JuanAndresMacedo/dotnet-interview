using TodoApi.Dtos.TodoItemDtos;

namespace TodoApi.BusinessLogic.TodoItems
{
    public interface ITodoItemService
    {
        Task<TodoItemDto?> CreateAsync(long todoListId, CreateTodoItem payload);
        Task<TodoItemDto?> UpdateDescriptionAsync(long todoListId,
            long todoItemId, UpdateTodoItem payload);
        Task<bool> CompleteAsync(long todoListId, long todoItemId);
        Task<bool> DeleteAsync(long todoListId, long todoItemId);
    }
}
