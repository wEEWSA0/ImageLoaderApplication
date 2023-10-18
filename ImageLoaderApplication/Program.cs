using ImageLoaderApplication.Data;
using ImageLoaderApplication.Model;
using ImageLoaderApplication.Repository;
using ImageLoaderApplication.Util;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;

var builder = WebApplication.CreateBuilder(args);

var defaultConnection = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

var connectionString = defaultConnection is null ? builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.") : defaultConnection;

var services = builder.Services;

var config = builder.Configuration;

services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o => {
    o.TokenValidationParameters = new()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JwtSettings:Issuer"]!,
        ValidAudience = config["JwtSettings:Audience"]!,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!))
    };
});
services.AddAuthorization();

services.AddControllers();

services.AddDbContext<ApplicationDbContext>(options =>
    options.EnableDetailedErrors()
    .EnableSensitiveDataLogging()
    .UseNpgsql(connectionString, o =>
        {
            o.EnableRetryOnFailure(2);
        })
    );

services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

services.AddSingleton<JwtTokenUtil>(x => 
    new (config["JwtSettings:Key"]!,
    int.Parse(config["JwtSettings:ExpiredInMinutes"]!),
    config["JwtSettings:Issuer"]!, 
    config["JwtSettings:Audience"]!));

services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Jwt Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Bearer {jwt_token}",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

services.AddScoped<FriendsRepository>();
services.AddScoped<ImageRepository>();
services.AddScoped<UserRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.Run();
