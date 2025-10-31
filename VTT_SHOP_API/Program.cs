using VTT_SHOP_CORE.DTOs;
using VTT_SHOP_CORE.Services;
using VTT_SHOP_CORE.Services.AuthService;
using VTT_SHOP_DATABASE;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.Extensions;
using VTT_SHOP_SHARED.Interfaces.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDatabase<VTTShopDBContext>(connectionString,nameof(VTT_SHOP_DATABASE));
builder.Services.RegisterRepositories("VTT_SHOP");
builder.Services.RegisterServices("VTT_SHOP");
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<JWTService>();
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

app.Run();
