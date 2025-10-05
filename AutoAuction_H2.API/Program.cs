using AutoAuction_H2.Models.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AuctionDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// Add controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AutoAuction API",
        Version = "v1"
    });

    // Gruppér kun efter controller-navne
    c.TagActionsBy(api => new[] { api.ActionDescriptor.RouteValues["controller"] ?? "default" });

    // Undgå at lave en ekstra "AutoAuction_H2.API" sektion
    c.DocInclusionPredicate((docName, apiDesc) => true);
});

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AutoAuction API v1");
        c.RoutePrefix = string.Empty; // gør at swagger kører direkte på https://localhost:44334/
    });
}

// Redirect root til swagger hvis du beholder prefix
app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
