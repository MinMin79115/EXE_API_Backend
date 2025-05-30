using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using EXE_API_Backend.Repositories.Interface;
using EXE_API_Backend.Repositories.Implement;
using EXE_API_Backend.Models.Database;
using EXE_API_Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add CORS - Đọc từ configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy",
        policy =>
        {
            // Đọc allowed origins từ appsettings.json
            var corsSettings = builder.Configuration.GetSection("CorsSettings");
            var allowedOrigins = corsSettings.GetSection("AllowedOrigins").Get<string[]>() 
                ?? new[] { "http://localhost:3000" }; // Default nếu không config
            
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "EXE API Backend", 
        Version = "v1", 
        Description = "Backend API for EXE application, providing various endpoints for system operations.",
    });
});

// Register MongoDB services
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<ExeApiDBContext>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IEmailHistoryRepository, EmailHistoryRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register RefreshTokenService
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// Register EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

// Register EmailTemplateService
builder.Services.AddScoped<EmailTemplateService>();

// Register JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])
        )
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EXE API Backend v1");
    c.DocumentTitle = "EXE API Swagger UI";
    c.DefaultModelsExpandDepth(-1);
});

app.UseHttpsRedirection();

// Add CORS middleware - Luôn dùng ReactPolicy
app.UseCors("ReactPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/ping", () => "Skibidi bop bop yes yes!");

app.Run();
