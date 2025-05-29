using TodoApi.Dtos.TodoListDtos;

namespace TodoApi.BusinessLogic.TodoLists
{
    public interface ITodoListService
    {
        Task<TodoListDto> CreateAsync(CreateTodoList payload);
        Task<IList<TodoListDto>> GetAllAsync();
        Task<TodoListDto?> GetByIdAsync(long id);
        Task<TodoListDto?> UpdateAsync(long id, UpdateTodoList payload);
        Task<bool> DeleteAsync(long id);
    }
}
