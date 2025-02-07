using BlogPlatformApi.Data;
using BlogPlatformApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSqlite<BlogPlatformContext>(connString);

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapArticleEndpoints();
app.MapTagEndpoints();

app.Run();
