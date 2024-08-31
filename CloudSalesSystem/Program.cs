using CloudSalesSystem.DBContext;
using CloudSalesSystem.HelperClasses;
using CloudSalesSystem.Interfaces;
using CloudSalesSystem.Services.CCPService;
using CloudSalesSystem.Services.CurrentCustomerService;
using CloudSalesSystem.Services.LoginService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMockHttpClient();
var connectionString = builder.Configuration.GetConnectionString("CloudSalesSytem");

builder.Services.AddDbContext<CloudSalesSystemDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<ICCPService, CCPService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
});
builder.Services.AddTransient<ICurrentCustomerService, CurrentCustomerService>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<RouteOptions>(options =>
 {
     options.LowercaseUrls = true;
     options.LowercaseQueryStrings = true;
 });
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CloudSalesSytem",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
    {
        var scopedContext = scope.ServiceProvider.GetRequiredService<CloudSalesSystemDbContext>();
        SeedDatabase.Initialize(scopedContext);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
