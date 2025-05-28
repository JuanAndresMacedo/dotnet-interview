using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.BusinessLogic.TodoLists;
using TodoApi.Dtos;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/todolists")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ITodoListService _todoListService;

        public TodoListsController(TodoContext context, ITodoListService todoListService)
        {
            _context = context;
            _todoListService = todoListService;
        }

        // GET: api/todolists
        [HttpGet]
        public async Task<ActionResult<IList<TodoList>>> GetTodoLists()
        {
            return Ok(await _todoListService.GetAllAsync());
        }

        // GET: api/todolists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoList(long id)
        {
            var todoList = await _todoListService.GetByIdAsync(id);
            
            if (todoList == null)
                return NotFound();

            return Ok(todoList);
        }

        // PUT: api/todolists/5
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTodoList(long id, UpdateTodoList payload)
        {
            var todoListUpdated = await _todoListService.UpdateAsync(id, payload);

            if (todoListUpdated == null)
                return NotFound();

            return Ok(todoListUpdated);
        }

        // POST: api/todolists
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoListDto>> PostTodoList(CreateTodoList payload)
        {
            TodoListDto created = await _todoListService.CreateAsync(payload);

            return CreatedAtAction(
                "GetTodoList",
                new { id = created.Id },
                created
            );
        }

        // DELETE: api/todolists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoList(long id)
        {
            var todoListDeleted = await _todoListService.DeleteAsync(id);

            if (!todoListDeleted)
                return NotFound();

            return NoContent();
        }

        private bool TodoListExists(long id)
        {
            return (_context.TodoList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
