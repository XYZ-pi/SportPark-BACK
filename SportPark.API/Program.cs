using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SportPark.BusinessLogic;
using SportPark.DataAccess.Context;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите: Bearer {токен}"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// База данных
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Сервисы (Dependency Injection)
var jwtSecretKey = builder.Configuration["Jwt:SecretKey"]!;
builder.Services.AddScoped(sp => new TokenService(jwtSecretKey));
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ServiceManagementService>();
builder.Services.AddScoped<TrainerManagementService>();
builder.Services.AddScoped<ClassSessionManagementService>();
builder.Services.AddScoped<BookingManagementService>();
builder.Services.AddScoped<ContactRequestManagementService>();
builder.Services.AddScoped<SubscriptionManagementService>();
builder.Services.AddScoped<PersonalSessionManagementService>();
builder.Services.AddScoped<UserManagementService>();


// JWT-аутентификация
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await SportPark.DataAccess.Context.DbSeeder.SeedAsync(context);
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");
//Важный момент про безопасность: AllowAnyOrigin() — это нормально для учебного проекта и разработки, но для реального продакшена стоило бы ограничить конкретным доменом твоего сайта вместо "разрешить всем". Раз дедлайн жмёт и это учебный проект — сейчас это ок, но стоит об этом знать.

app.UseAuthentication(); // обязательно ДО UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();