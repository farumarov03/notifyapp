using NotifyService.src;
using NotifyService.src.Managers;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// redis connection 
builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
{
    return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"));
});

// register notify service
builder.Services.AddScoped<INotifyService, NotifyManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//server send event endpoint
app.MapGet("/sse/{id}", async (
    string id, INotifyService notify) => SSEResult.From(notify.ProduceEvents(id)
    ));

app.Run();

