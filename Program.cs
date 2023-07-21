using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

/* 
 * REFER HERE TO KNOW HOW ITS DONE
 * https://www.youtube.com/watch?v=v7q3pEK1EA0&t=1697s
 * https://www.youtube.com/watch?v=TDY_DtTEkes&t=638s
 */

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Inside AddSwaggerGen whatever is written 
// is to add the token to the header when used for authentication.
// In the inspect -> header it can be found with the name of Authorization as mentioned here.
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authotrization header using the Bearer Scheme (\"bearer {token)\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

// This depedency has been injected for JWT authentication
// What is the use of JWT bearer token?
// JWT Bearer authentication is a way to secure an application by validating the authenticity of a JSON Web Token (JWT)
// that is sent in the request header
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer( options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// UseAuthentication should be done before UseAuthorization
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
