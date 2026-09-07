using Microsoft.EntityFrameworkCore;
using RecipeCosting.Api.Data;
using RecipeCosting.Api.Helpers;
using RecipeCosting.Api.Services.Auth;
using RecipeCosting.Api.Services.Baker;
using RecipeCosting.Api.Services.Ingredients;
using RecipeCosting.Api.Services.Products;

const string BrowserClient = "BrowserClient";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<UnhandledExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDocumentedSwagger();
builder.Services.AddTokenAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddScoped<IBakerService, BakerService>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddDbContext<RecipeCostingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("RecipeCostingConnection")));

builder.Services.AddHealthChecks().AddDbContextCheck<RecipeCostingDbContext>("database");

var browserOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];

builder.Services.AddCors(options =>
    options.AddPolicy(BrowserClient, policy => policy
        .WithOrigins(browserOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

if (app.Configuration.GetValue<bool>("Database:MigrateOnStart"))
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<RecipeCostingDbContext>().Database.MigrateAsync();
}

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe Costing API v1"));
}

app.UseCors(BrowserClient);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

/// <summary>
/// Named so that the test host can start the same application the server does.
/// </summary>
public partial class Program;
