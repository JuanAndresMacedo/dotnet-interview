using Microsoft.AspNetCore.Mvc;
using TodoApi.BusinessLogic.TodoItems;
using TodoApi.Dtos.TodoItemDtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/todolists/{todoListId}/items")]
    [ApiController]
    public class TodoItemController : ControllerBase
    {
        private readonly ITodoItemService _todoItemService;

        public TodoItemController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

        [HttpGet("{todoItemId}")]
        public async Task<ActionResult<TodoList>> GetTodoItem(long todoListId,
            long todoItemId)
        {
            var todoItem = await _todoItemService.GetByIdAsync(todoListId, todoItemId);

            if (todoItem == null)
                return NotFound();

            return Ok(todoItem);
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> PostTodoItem(long todoListId,
            CreateTodoItem payload)
        {
            var created = await _todoItemService.CreateAsync(todoListId, payload);

            if (created == null)
                return NotFound("La lista no fue encontrada");

            return CreatedAtAction(
                "GetTodoItem",
                new { todoListId = todoListId, todoItemId = created.Id },
                created
            );
        }

        [HttpPut("{todoItemId}")]
        public async Task<ActionResult<TodoItemDto>> PutTodoItemDescription(
            long todoListId,
            long todoItemId,
            UpdateTodoItem payload)
        {
            var updated = await _todoItemService.UpdateDescriptionAsync(
                todoListId, todoItemId, payload);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }

        [HttpPut("{itemId}/complete")]
        public async Task<ActionResult> PutTodoItemIsCompleted(
            long todoListId,
            long todoItemId)
        {
            var updated = await _todoItemService.CompleteAsync(todoListId, todoItemId);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{todoItemId}")]
        public async Task<ActionResult> DeleteItem(
            long todoListId,
            long todoItemId)
        {
            var deleted = await _todoItemService.DeleteAsync(todoListId, todoItemId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
