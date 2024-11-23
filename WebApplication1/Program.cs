using App.Business;
using App.Business.Exceptions;
using App.Business.Services;
using App.Data;
using App.Data.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text;
using WebApplication1.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
});
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseNpgsql(connectionString)
);
//Adding automapper
builder.Services.AddAutoMapper(typeof(Mapper));
//Adding generic repository
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
//Adding generic services
builder.Services.AddScoped(typeof(IGenericServiceAsync<,>),typeof(GenericService<,>));
builder.Services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["PrivateKey"])),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddTransient(typeof(IJwtService),typeof(JwtService));
builder.Services.AddAuthorization();

builder.Services.AddScoped(typeof(IUserService),typeof(UserService));
builder.Services.AddScoped(typeof(IPostService),typeof(PostService));
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
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (BusinessExceptionHandler ex)
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsJsonAsync(new 
        {
            StatusCode = ex.StatusCode,
            Message = ex.Message,

        });
    }
});

app.Run();
