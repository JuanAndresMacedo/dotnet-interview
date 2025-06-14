using Microsoft.EntityFrameworkCore;
using TodoApi.BusinessLogic.TodoItems;
using TodoApi.BusinessLogic.TodoLists;

var builder = WebApplication.CreateBuilder(args);
builder
    .Services.AddDbContext<TodoContext>(opt =>
        opt.UseSqlServer(builder.Configuration.GetConnectionString("TodoContext"))
    )
    .AddScoped<ITodoListService, TodoListService>()
    .AddScoped<ITodoItemService, TodoItemService>()
    .AddEndpointsApiExplorer()
    .AddControllers();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseAuthorization();
app.MapControllers();
app.Run();
