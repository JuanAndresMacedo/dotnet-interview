using TodoApi.Dtos.TodoItemDtos;

namespace TodoApi.Dtos.TodoListDtos
{
    public class TodoListDto
    {
        public long Id { get; set; }
        public required string Name { get; set; }
        public List<TodoItemDto> Items { get; set; } = new();
    }
}
