using Microsoft.EntityFrameworkCore;
using WebSocketChat.Api.Controllers.Room;
using WebSocketChat.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var config = builder.Configuration;
var connectionString = config["DefaultConnection"];

builder.Services.AddDbContext<MessageDbContext>(options => options.UseNpgsql(connectionString).LogTo(Console.WriteLine),
    ServiceLifetime.Singleton);
builder.Services.AddSingleton<IMessageDataSource, MessageDataSource>();
builder.Services.AddSingleton<RoomController>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var options = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(options);

app.Run();