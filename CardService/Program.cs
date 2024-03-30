using CardService.src;
using CardService.src.Managers;
using CardService.src.Models;
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

// register card service
builder.Services.AddScoped<ICardEvent, CardEventManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//card event endpoint
app.MapPost("/events", async (string userId, EventModel model, ICardEvent _card) => {
    //generate transaction id
    var transactionId = Guid.NewGuid().ToString();
    // run async backgroud task
    _ = Task.Run(async () =>
    {
        await _card.SaveCardEvent(userId, model, transactionId);
    });
    return transactionId;
});

app.Run();

