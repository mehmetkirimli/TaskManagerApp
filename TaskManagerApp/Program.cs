using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using TaskManagerApp.Data;
using TaskManagerApp.Entity;
using TaskManagerApp.Mapper;
using TaskManagerApp.Repository.Impl;
using TaskManagerApp.Repository;
using TaskManagerApp.Service.Impl;
using TaskManagerApp.Service;
using TaskManagerApp.Middleware;
using TaskManagerApp.Utils;
using TaskManagerApp.Validator;
using FluentValidation;
using TaskManagerApp.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); // Generic repo
builder.Services.AddScoped<ITaskDataService, TaskDataService>(); // Task service
builder.Services.AddScoped<IAuthService, AuthService>(); // Auth service
builder.Services.AddScoped<AuthorizeUserAttribute>();

// Middleware İçin :Key :Isuuers ve diğer ayarlar appsettings.json dosyasından alınacak
builder.Services.AddSingleton(sp => builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured."));
builder.Services.AddSingleton(sp => builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured."));

#region CORS VE FRONT-END
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Angular'ın çalıştığı URL
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
#endregion




builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options => 
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Task Manager API",
        Version = "1.0.0",
        Description = "Task Manager API with JWT Authentication",
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Token 'Bearer Token formatında girilecek ."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

#region AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
#endregion

#region JWT

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

#endregion

#region DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskManager"));
});

#endregion

#region Identity Tanımlama
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();
#endregion

#region Log

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();// Uygulamada loglama yapabilmek için

#endregion

#region Fluent Validation
//builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>(); // bu çalışmadı
builder.Services.AddScoped<IValidator<RegisterDto>, RegisterValidator>();
#endregion

var app = builder.Build();


// Middleware sırası önemli
app.UseMiddleware<TokenValidationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => 
    { 
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager API V1.0.0");
        options.RoutePrefix = string.Empty;
    }); 
}

app.UseHttpsRedirection();
app.UseCors(MyAllowSpecificOrigins); // CORS Middleware ekle
app.UseAuthentication(); // Kimlik doğrulama middleware etkinleştiriyoruz. #Yeni 
app.UseAuthorization();
app.MapControllers();
app.Run();
