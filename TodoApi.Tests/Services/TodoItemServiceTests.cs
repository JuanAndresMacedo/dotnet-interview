using Microsoft.EntityFrameworkCore;
using TodoApi.BusinessLogic.TodoItems;
using TodoApi.Dtos.TodoItemDtos;
using TodoApi.Models;

namespace TodoApi.Tests.Services
{
    public class TodoItemServiceTests : IDisposable
    {
        private readonly TodoContext _context;
        private readonly TodoItemService _service;

        public TodoItemServiceTests()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            _context = new TodoContext(options);
            _context.TodoList.AddRange(
                new TodoList { Id = 1, Name = "Lista 1" },
                new TodoList { Id = 2, Name = "Lista 2" }
            );
            _context.TodoItem.Add(
                new TodoItem{
                    Id = 1,
                    Description = "Tarea 1",
                    TodoListId = 1
                }
            );
            _context.SaveChanges();

            _service = new TodoItemService(_context);
        }

        public void Dispose() 
        {
            _context.Dispose();
        }

        [Fact]
        public async Task CreateAsync_WhenCalledAndListExists_CreatesTodoItem()
        {
            var payload = new CreateTodoItem {
                Description = "Nueva tarea"
            };

            var result = await _service.CreateAsync(1, payload);

            Assert.NotNull(result);
            Assert.Equal(payload.Description, result.Description);
            Assert.False(result.IsCompleted);
            Assert.True(_context.TodoItem.Any(i => i.Id == result.Id && i.TodoListId == 1));
        }

        [Fact]
        public async Task GetByIdAsync_WhenExists_ReturnsTodoItem()
        {
            TodoItem todoItem = _context.TodoItem.First();

            var result = await _service.GetByIdAsync(todoItem.TodoListId, todoItem.Id);

            Assert.NotNull(result);
            Assert.Equal(todoItem.Id, result.Id);
            Assert.Equal(todoItem.Description, result.Description);
        }

        [Fact]
        public async Task Update_WhenCalled_UpdatesTodoItem()
        {
            TodoItem todoItemToUpdate = _context.TodoItem.First();
            UpdateTodoItem payload = new UpdateTodoItem
            {
                Description = "Nueva descripción"
            };

            var result = await _service.UpdateDescriptionAsync(todoItemToUpdate.TodoListId,
                todoItemToUpdate.Id, payload);

            Assert.NotNull(result);
            Assert.Equal(todoItemToUpdate.Id, result.Id);
            Assert.Equal("Nueva descripción", result.Description);

            var entity = await _context.TodoItem.FindAsync(todoItemToUpdate.Id);
            Assert.NotNull(entity);
            Assert.Equal("Nueva descripción", entity.Description);
        }

        [Fact]
        public async Task Complete_WhenCalled_CompletesTodoItem()
        {
            TodoItem todoItemToComplete = _context.TodoItem.First();

            var result = await _service.CompleteAsync(todoItemToComplete.TodoListId,
                todoItemToComplete.Id);

            Assert.True(result);

            var entity = await _context.TodoItem.FindAsync(todoItemToComplete.Id);
            Assert.NotNull(entity);
            Assert.True(entity.IsCompleted);
        }


        [Fact]
        public async Task Delete_WhenCalled_DeletesTodoList()
        {
            TodoItem todoItemToDelete = _context.TodoItem.First();

            bool result = await _service.DeleteAsync(todoItemToDelete.TodoListId,
                todoItemToDelete.Id);

            Assert.True(result);
            Assert.Null(await _context.TodoItem.FindAsync(todoItemToDelete.Id));
        }
    }
}
