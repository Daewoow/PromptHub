using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using WebApplication1;
using WebApplication1.Cache;
using WebApplication1.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<PromptsCrudRepository>();
builder.Services.AddScoped<CacheService>();
builder.Services.AddScoped<PromptService>();
builder.Services.AddScoped<RegisterRequest>();
builder.Services.AddControllers();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "http://localhost:5160/",
            ValidAudience = "http://localhost:5184/",
            IssuerSigningKey = new SymmetricSecurityKey("mega_hyper_ultra_penta_hepto_secret_key_123"u8.ToArray())
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});
builder.Services.AddAuthenticationCore();

var app = builder.Build();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapPost("/api/auth/register", async context =>
{
    var request = await context.Request.ReadFromJsonAsync<RegisterRequest>();

    if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Имя пользователя и пароль обязательны.");
        return;
    }

    if (new UsersCrudRepository().UserExists(request.Username))
    {
        context.Response.StatusCode = StatusCodes.Status409Conflict;
        await context.Response.WriteAsync("Пользователь с таким именем уже существует.");
        return;
    }

    var role = new[] { "User" };
    if (new[] { "Лера", "Максим", "Саша" }.Contains(request.Username))
    {
        role = new[] { "Admin" };
    }

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, request.Username),
        new Claim(ClaimTypes.Role, role[0]) 
    };
    
    new UsersCrudRepository().SaveUser(request.Username, request.Password);

    var jwt = new JwtSecurityToken(
        issuer: "MyAuthServer",
        audience: "MyAuthClient",
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            ("mysupersecret_secretsecretsecretkey!123")), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsJsonAsync(new { Token = encodedJwt });
});

app.MapPost("/api/auth/login", async context =>
{
    var request = await context.Request.ReadFromJsonAsync<RegisterRequest>();
    await using var db = new ApplicationContext();

    if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Имя пользователя и пароль обязательны.");
        return;
    }

    var user = db.Users.FirstOrDefault(user => user.UserName == request.Username);
    if (user is null)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("Пользователь не найден.");
        return;
    }

    if (user.UserPassword != request.Password)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Неверный пароль.");
        return;
    }

    var claims = new List<Claim> {new Claim(ClaimTypes.Name, user.UserName) };
    var jwt = new JwtSecurityToken(
        issuer: "MyAuthServer",
        audience: "MyAuthClient",
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            ("mysupersecret_secretsecretsecretkey!123")), SecurityAlgorithms.HmacSha256));
    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 
    context.Response.StatusCode = StatusCodes.Status200OK;
    await context.Response.WriteAsJsonAsync(new { Token = encodedJwt });
});

app.MapGet("api/todo", async ([FromServices] PromptService toDoService) 
    => await toDoService.ReadAllAsync());
    
app.MapPost("api/todo", async ([FromBody] Prompt item, [FromServices] PromptService toDoService) 
    => await toDoService.CreateAsync(item));
 
app.MapPut("api/todo", async ([FromBody] Prompt item, [FromServices] PromptService toDoService) 
    => await toDoService.UpdateAsync(item));
 
app.MapGet("api/todo/{id}", async (string id, [FromServices] PromptService toDoService) 
    => await toDoService.ReadByIdAsync(int.Parse(id)));
 
app.MapDelete("api/todo/{id}", async (string id, [FromServices] PromptService toDoService)
    => await toDoService.DeleteAsync(int.Parse(id)));

app.Run();
