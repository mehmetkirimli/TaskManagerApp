using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using TaskManagerApp.Data;
using TaskManagerApp.Entity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( options => 
{
    options.SwaggerDoc("VERSIYON 1.0.0", new OpenApiInfo
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

// Validators
//builder.Services.AddValidatorsFromAssemblyContaining<Program>();
//builder.Services.AddFluentValidation();

#region Log

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs\\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();// Uygulamada loglama yapabilmek için

#endregion

var app = builder.Build();

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
app.UseAuthentication(); // Kimlik doğrulama middleware etkinleştiriyoruz. #Yeni 
app.UseAuthorization();
app.MapControllers();
app.Run();
