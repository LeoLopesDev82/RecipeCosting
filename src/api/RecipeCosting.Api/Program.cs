using Microsoft.EntityFrameworkCore;
using RecipeCosting.Api.Data;
using RecipeCosting.Api.Helpers;
using RecipeCosting.Api.Services.Auth;
using RecipeCosting.Api.Services.Baker;
using RecipeCosting.Api.Services.Ingredients;
using RecipeCosting.Api.Services.Products;

const string AngularDevServer = "AngularDevServer";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

builder.Services.AddCors(options =>
    options.AddPolicy(AngularDevServer, policy => policy
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipe Costing API v1"));
    app.UseCors(AngularDevServer);
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
