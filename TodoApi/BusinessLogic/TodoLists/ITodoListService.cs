using TodoApi.Dtos;

namespace TodoApi.BusinessLogic.TodoLists
{
    public interface ITodoListService
    {
        Task<TodoListDto> CreateAsync(CreateTodoList payload);
        Task<IList<TodoListDto>> GetAllAsync();
    }
}
