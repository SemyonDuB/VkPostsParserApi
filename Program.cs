using VkServer.Data;
using VkServer.Repositories;
using VkServer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddNpgsql<UserResultContext>(builder.Configuration.GetConnectionString("UserResultsDB"));
builder.Services.AddHttpClient();

builder.Services.AddTransient<IVkApiService, VkApiService>();
builder.Services.AddScoped<IUserResultRepository, UserResultRepository>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();