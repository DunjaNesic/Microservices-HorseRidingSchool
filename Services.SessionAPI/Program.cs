using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.HorseAPI.Infrastructure.Mapping;
using Services.SessionAPI.ApplicationLayer;
using Services.SessionAPI.ApplicationLayer.IService;
using Services.SessionAPI.Domain.Contracts;
using Services.SessionAPI.Infrastructure;
using Services.SessionAPI.Infrastructure.Implementations;
using Services.SessionAPI.Utilities;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SessionDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(SessionProfile));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<IHorseService, HorseService>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ApiAuthHttpClientHandler>();
builder.Services.AddAutoMapper(typeof(SessionProfile));
builder.Services.AddSingleton<IRabbitMQConnection, RabbitMQConnection>();
builder.Services.AddScoped<IMessageProducer, RabbitMQProducer>();


builder.Services.AddHttpClient("Horse", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:HorseAPI"])).AddHttpMessageHandler<ApiAuthHttpClientHandler>();
builder.Services.AddHttpClient("Trainer", u => u.BaseAddress = new Uri(builder.Configuration["ServiceUrls:TrainerAPI"])).AddHttpMessageHandler<ApiAuthHttpClientHandler>(); ;


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = " \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] {}
        }
    });
});

var secret = builder.Configuration.GetValue<string>("ApiSettings:Secret");
var issuer = builder.Configuration.GetValue<string>("ApiSettings:Issuer");
var audience = builder.Configuration.GetValue<string>("ApiSettings:Audience");

var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        ValidateAudience = true,
    };
});

builder.Services.AddAuthorization();


var app = builder.Build();

var rabbitConnection = app.Services.GetRequiredService<IRabbitMQConnection>();
await rabbitConnection.InitializeAsync();

// Configure the HTTP request pipeline.
    app.UseSwagger();
    app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
