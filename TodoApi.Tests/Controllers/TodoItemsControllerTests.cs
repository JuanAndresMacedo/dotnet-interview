using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoApi.BusinessLogic.TodoItems;
using TodoApi.Controllers;
using TodoApi.Dtos.TodoItemDtos;

namespace TodoApi.Tests.Controllers
{
    public class TodoItemsControllerTests
    {
        private readonly Mock<ITodoItemService> _serviceMock;
        private readonly TodoItemController _controller;

        public TodoItemsControllerTests()
        {
            _serviceMock = new Mock<ITodoItemService>();
            _controller = new TodoItemController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetTodoItem_WhenCalled_ReturnsTodoItemDto()
        {
            TodoItemDto dto = new TodoItemDto
            {
                Id = 1,
                Description = "Tarea",
                IsCompleted = false
            };

            _serviceMock.Setup(s => s.GetByIdAsync(2, 1)).ReturnsAsync(dto);

            var result = await _controller.GetTodoItem(2, 1);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostTodoItem_WhenCalled_CreatesTodoItem()
        {
            TodoItemDto created = new TodoItemDto
            {
                Id = 2,
                Description = "Tarea1",
                IsCompleted = false
            };

            CreateTodoItem payload = new CreateTodoItem
            {
                Description = "Tarea1"
            };

            _serviceMock.Setup(s => s.CreateAsync(1, It.IsAny<CreateTodoItem>()))
                .ReturnsAsync(created);

            var result = await _controller.PostTodoItem(1, payload);

            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetTodoItem), createdResult.ActionName);
            Assert.Equal(1L, createdResult.RouteValues["todoListId"]);
            Assert.Equal(2L, createdResult.RouteValues["todoItemId"]);
            Assert.Equal(created, createdResult.Value);
        }

        [Fact]
        public async Task PutTodoItemDescription_WhenCalled_UpdatesTodoItem()
        {
            var updated = new TodoItemDto
            {
                Id = 1,
                Description = "Tarea Actualizada",
                IsCompleted = false
            };

            var payload = new UpdateTodoItem
            {
                Description = "Tarea Actualizada"
            };

            _serviceMock.Setup(s => s.UpdateDescriptionAsync(2, 1,
                It.IsAny<UpdateTodoItem>())).ReturnsAsync(updated);

            var result = await _controller.PutTodoItemDescription(2, 1, payload);

            var ok = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(updated, ok.Value);
        }

        [Fact]
        public async Task PutTodoItemIsCompleted_WhenCalled_UpdatesTodoItem()
        {
            _serviceMock.Setup(s => s.CompleteAsync(1, 2)).ReturnsAsync(true);

            var result = await _controller.PutTodoItemIsCompleted(1, 2);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteItem_WhenCalled_ReturnsTrue()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1, 2)).ReturnsAsync(true);

            var result = await _controller.DeleteItem(1, 2);
            Assert.IsType<NoContentResult>(result);
        }
    }
}
