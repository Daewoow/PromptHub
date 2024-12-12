using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using StackExchange.Redis;
using WebApplication1;
using WebApplication1.Cache;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var store = new DocumentStore
{
    Urls = ["http://localhost:8080"],
    Database = "Prompts"
};

store.Initialize();
builder.Services.AddSingleton<IDocumentStore>(store);
builder.Services.AddScoped<CrudRepository>();
builder.Services.AddScoped<CacheService>();
builder.Services.AddScoped<PromptService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("api/todo", async ([FromServices] PromptService toDoService) 
    => await toDoService.ReadAllAsync());
    
app.MapPost("api/todo", async ([FromBody] Prompt item, [FromServices] PromptService toDoService) 
    => await toDoService.CreateAsync(item));
 
app.MapPut("api/todo", async ([FromBody] Prompt item, [FromServices] PromptService toDoService) 
    => await toDoService.UpdateAsync(item));
 
app.MapGet("api/todo/{id}", async (string id, [FromServices] PromptService toDoService) 
    => await toDoService.ReadByIdAsync(id));
 
app.MapDelete("api/todo/{id}", async (string id, [FromServices] PromptService toDoService)
    => await toDoService.DeleteAsync(id));
    
app.Run();
