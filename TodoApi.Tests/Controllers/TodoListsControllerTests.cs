using Microsoft.AspNetCore.Mvc;
using TodoApi.BusinessLogic.TodoLists;
using TodoApi.Controllers;
using Moq;
using TodoApi.Dtos.TodoListDtos;

namespace TodoApi.Tests;

#nullable disable
public class TodoListsControllerTests
{
    private readonly Mock<ITodoListService> _serviceMock;
    private readonly TodoListsController _controller;

    public TodoListsControllerTests()
    {
        _serviceMock = new Mock<ITodoListService>();
        _controller = new TodoListsController(_serviceMock.Object);
    }

    private List<TodoListDto> InitilizeSampleLists()
    {
        return new List<TodoListDto>
            {
                new() { Id = 1, Name = "Lista 1" },
                new() { Id = 2, Name = "Lista 2" }
            };
    }

    [Fact]
    public async Task GetTodoList_WhenCalled_ReturnsTodoListList()
    {
        List<TodoListDto> list = InitilizeSampleLists();

        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(list);

        var actionResult = await _controller.GetTodoLists();

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        var returned = Assert.IsAssignableFrom<IList<TodoListDto>>(ok.Value);
        Assert.Equal(2, returned.Count);
    }

    [Fact]
    public async Task GetTodoList_WhenCalled_ReturnsTodoListById()
    {
        List<TodoListDto> list = InitilizeSampleLists();

        _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(list[0]);

        var actionResult = await _controller.GetTodoList(1);

        var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
        Assert.Equal(list[0], ok.Value);
    }

    [Fact]
    public async Task PutTodoList_WhenTodoListDoesntExist_ReturnsBadRequest()
    {
        var payload = new UpdateTodoList { Name = "X" };

        _serviceMock.Setup(s => s.UpdateAsync(5, payload))
            .ReturnsAsync((TodoListDto?)null);

        var result = await _controller.PutTodoList(5, payload);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task PutTodoList_WhenCalled_UpdatesTheTodoList()
    {
        var payload = new UpdateTodoList { Name = "Updated" };

        var updatedDto = new TodoListDto
        {
            Id = 1,
            Name = "Updated"
        };

        _serviceMock.Setup(s => s.UpdateAsync(1, payload)).ReturnsAsync(updatedDto);

        var result = await _controller.PutTodoList(1, payload);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(updatedDto, ok.Value);
    }

    [Fact]
    public async Task PostTodoList_WhenCalled_CreatesTodoList()
    {
        var payload = new CreateTodoList
        {
            Name = "Lista"
        };

        var createdDto = new TodoListDto
        {
            Id = 1,
            Name = "Lista"
        };

        _serviceMock.Setup(s => s.CreateAsync(payload)).ReturnsAsync(createdDto);

        var result = await _controller.PostTodoList(payload);

        var created = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(nameof(_controller.GetTodoList), created.ActionName);
        Assert.Equal(createdDto.Id, created.RouteValues["id"]);
        Assert.Equal(createdDto, created.Value);
    }

    [Fact]
    public async Task DeleteTodoList_WhenCalled_RemovesTodoList()
    {
        var createdDto = new TodoListDto
        {
            Id = 1,
            Name = "Lista"
        };

        _serviceMock.Setup(s => s.DeleteAsync(1)).ReturnsAsync(true);

        var result = await _controller.DeleteTodoList(1);

        Assert.IsType<NoContentResult>(result);
    }
}
