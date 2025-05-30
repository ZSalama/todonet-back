using Microsoft.EntityFrameworkCore;
using todo_back.Data;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);

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


builder.Services.AddDbContext<TodoBackDbContext>(opt =>
    opt.UseNpgsql(connString));


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", async (TodoBackDbContext db) =>
{
    var list = await db.Weathers
        .Select(w => new { w.Day, w.Temperature })
        .ToListAsync();
    return Results.Ok(list);
})
.WithName("GetWeatherForecast");

app.Run();

