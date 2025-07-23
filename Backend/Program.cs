using Microsoft.EntityFrameworkCore;
using Backend.Identity;
using Backend.Authentication;
using Backend.Core;
using Backend.Extentions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration["ConnectionStrings:Default"]));
builder.Services.AddIdentity(configuration);
builder.Services.AddAuthentication(configuration);
builder.Services.AddAuthorization(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStorageService, StorageService>();
builder.Services.AddScoped<IEmailSender<ApplicationUser>, EmailSenderDummy>();
if (builder.Environment.IsProduction())
{
    // I know i should store it at least encrypted, better in cloud storage
    builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("/keys/storage"));
}

// Migrations on startup:
var context = builder.Services.BuildServiceProvider().GetService<ApplicationDbContext>();
await context.Database.MigrateAsync();

var app = builder.Build();

if (app.Environment.IsEnvironment("DevelopmentLocal"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (app.Environment.IsEnvironment("DevelopmentLocal"))
{
    app.UseCors();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapSwagger();

var roleManager = app.Services.GetService<RoleManager<IdentityRole>>();
if (!await roleManager.RoleExistsAsync("Admin"))
{
    await roleManager.CreateAsync(new IdentityRole("Admin"));
}

app.Run();