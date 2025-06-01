using Microsoft.EntityFrameworkCore;
using todo_back.Data;
using todo_back.Models;



namespace todo_back.Endpoints
{
    public static class TodoEndpoints
    {
        public static void MapTodoEndpoints(this WebApplication app)
        {
            // You could also inject services via builder.Services and use them here.
            var group = app.MapGroup("/api/todo");

            group.MapGet("/", async (TodoBackDbContext db) =>
            {
                var list = await db.Todos
                    .Select(w => new { w.Title, w.Category, w.Description, w.Id, w.DueDate, w.CreatedAt })
                    .ToListAsync();
                return Results.Ok(list);
            })
            .WithName("GetAllTodos");

            // Posting a new todo entry
            group.MapPost("/", async (TodoBackDbContext db, Todo newTodo) =>
            {
                db.Todos.Add(newTodo);
                await db.SaveChangesAsync();
                return Results.Ok(newTodo);
            })
            .WithName("CreateTodo");

            // delete a todo
            group.MapDelete("/{id:int}", async (TodoBackDbContext db, int id) =>
            {
                var todo = await db.Todos.FindAsync(id);
                if (todo == null)
                {
                    return Results.NotFound();
                }

                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return Results.Ok(todo);
            })
            .WithName("DeleteTodo");
            
        
            // etc. Add more endpoints here...
        }
    }
}