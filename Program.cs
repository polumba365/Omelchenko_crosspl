using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Omelchenko_crosspl.Data;
using Omelchenko_crosspl.Models;
using Omelchenko_crosspl.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// 1. Подключение к базе данных (SQLite)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=arctic_oil.db"));

// 2. Регистрация нашего ЕДИНСТВЕННОГО сервиса DataService
builder.Services.AddScoped<DataService>();

// 3. Настройка JWT авторизации
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = AuthOptions.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthOptions.Key)),
            ValidateIssuerSigningKey = true,
            // Эта строка критически важна, чтобы работало [Authorize(Roles = "admin")]
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger оставим, он не мешает Бруно и полезен для отладки

var app = builder.Build();

// 4. Автоматическое создание базы данных и заполнение тестовыми данными при первом запуске
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// ВАЖНО: Сначала аутентификация (проверка токена), потом авторизация (проверка прав)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();