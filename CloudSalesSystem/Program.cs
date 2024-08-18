using CloudSalesSystem.DBContext;
using CloudSalesSystem.HelperClasses;
using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Services.CCPService;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMockHttpClient();
var connectionString = builder.Configuration.GetConnectionString("CloudSalesSytem");
builder.Services.AddDbContext<CloudSalesSystemDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ICCPService, CCPService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

await app.RunAsync();
