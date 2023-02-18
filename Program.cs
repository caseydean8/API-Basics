using System.Text;
using DotnetAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

// WebApplicationBuilder WebApplication.CreateBuilder(string[] args) (+ 2 overloads)
// Initializes a new instance of the `WebApplicationBuilder` class with preconfigured defaults\.
// Returns: The `WebApplicationBuilder`\.
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors((options) =>
    {
        options.AddPolicy("DevCors", (corsBuilder) =>
            {
                // default ports for angular, react, vue
                corsBuilder.WithOrigins("http://localhost:4200", "http://localhost:3000", "http://localhost:8080")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        options.AddPolicy("ProdCors", (corsBuilder) =>
            {
                corsBuilder.WithOrigins("https://myProductionSite.com")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
    });

builder.Services.AddScoped<IUserRepository, UserRepository>();

string? tokenKeyString = builder.Configuration.GetSection("AppSettings:TokenKey").Value;

// (extension) Microsoft.AspNetCore.Authentication.AuthenticationBuilder ...
// IServiceCollection.AddAuthentication(string defaultScheme) (+ 2 overloads)
// Registers services required by authentication services. 
// `defaultScheme` specifies the name of the scheme to use by default when a specific scheme isn't requested ...
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// Returns:  A `Microsoft.AspNetCore.Authentication.AuthenticationBuilder` that 
// can be used to further configure authentication\.
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                // for .NET 7. tokenKeyString null handler.
                tokenKeyString != null ? tokenKeyString : ""
            )),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

// UseAuthentication must come before UseAuthorization or you will get 401 errors
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
