using Microsoft.EntityFrameworkCore;
using TodoApi.BusinessLogic.TodoLists;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Tests.Services
{
    public class TodoListServiceTests : IDisposable
    {
        private readonly TodoContext _context;
        private readonly TodoListService _service;

        public TodoListServiceTests()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

            _context = new TodoContext(options);
            _context.TodoList.AddRange(
                new TodoList { Id = 1, Name = "Lista 1" },
                new TodoList { Id = 2, Name = "Lista 2" }
            );
            _context.SaveChanges();

            _service = new TodoListService(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task GetById_WhenCalled_ReturnsTodoListById()
        {
            TodoList todoList = _context.TodoList.First();

            var result = await _service.GetByIdAsync(todoList.Id);

            Assert.NotNull(result);
            Assert.Equal(todoList.Id, result.Id);
            Assert.Equal(todoList.Name, result.Name);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsAllTodoLists()
        {
            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, dto => dto.Name == "Lista 1");
            Assert.Contains(result, dto => dto.Name == "Lista 2");
        }

        [Fact]
        public async Task Create_WhenCalled_CreatesTodoList()
        {
            CreateTodoList payload = new CreateTodoList
            {
                Name = "Lista nueva"
            };

            TodoListDto result = await _service.CreateAsync(payload);

            Assert.Equal("Lista nueva", result.Name);

            var entity = await _context.TodoList.FindAsync(result.Id);
            Assert.NotNull(entity);
            Assert.Equal("Lista nueva", entity.Name);
        }

        [Fact]
        public async Task Update_WhenCalled_UpdatesTodoList()
        {
            TodoList toUpdate = _context.TodoList.First();
            UpdateTodoList payload = new UpdateTodoList
            {
                Name = "Lista actualizada"
            };

            var result = await _service.UpdateAsync(toUpdate.Id, payload);

            Assert.NotNull(result);
            Assert.Equal(toUpdate.Id, result.Id);
            Assert.Equal("Lista actualizada", result.Name);

            var entity = await _context.TodoList.FindAsync(toUpdate.Id);
            Assert.NotNull(entity);
            Assert.Equal("Lista actualizada", entity.Name);
        }

        [Fact]
        public async Task Delete_WhenCalled_DeletesTodoList()
        {
            TodoList toDelete = _context.TodoList.First();

            bool result = await _service.DeleteAsync(toDelete.Id);

            Assert.True(result);
            Assert.Null(await _context.TodoList.FindAsync(toDelete.Id));
        }
    }
}
