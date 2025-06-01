using Microsoft.EntityFrameworkCore;
using todo_back.Data;
using Npgsql;
using todo_back.Endpoints;


var builder = WebApplication.CreateBuilder(args);

// Define cors policies
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalAndVercel", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000", "https://todo-front-snowy.vercel.app")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


// Configure Entity Framework Core to use PostgreSQL
var raw = builder.Configuration.GetConnectionString("DefaultConnection");

string connString;

if (raw != null)
{
    var uri = new Uri(raw);
    var userInfo = uri.UserInfo.Split(':', 2);

    var npgBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Username = userInfo[0],
        Password = userInfo.Length > 1 ? userInfo[1] : null,
        Database = uri.AbsolutePath.TrimStart('/'),
        SslMode = SslMode.Require,
    };
    if (uri.Port > 0)
    {
        npgBuilder.Port = uri.Port;
    }

    connString = npgBuilder.ConnectionString;
}
else
{
    connString = Environment.GetEnvironmentVariable("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
}

// Register the DbContext with the connection string
builder.Services.AddDbContext<TodoBackDbContext>(opt =>
    opt.UseNpgsql(connString));


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseCors("AllowLocalAndVercel");

// only use OpenAPI in development mode
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


// Map the custom endpoints
app.MapTodoEndpoints();

app.Run();

