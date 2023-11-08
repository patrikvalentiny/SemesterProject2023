using infrastructure;
using infrastructure.DataModels;
using infrastructure.Repositories;
using Serilog;
using service.Password;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    //.WriteTo.Console()
    //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .CreateLogger();

Log.Information("Starting web application");

builder.Host.UseSerilog();

builder.Services.AddNpgsqlDataSource(Utilities.ProperlyFormattedConnectionString(Environment.GetEnvironmentVariable("databaseEnvVariableKey")!),
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
builder.Services.AddSingleton<IRepository<User>, UserRepository>();
builder.Services.AddSingleton<PasswordRepository>();
builder.Services.AddSingleton<PasswordHashAlgorithm>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseForwardedHeaders();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSecurityHeaders();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();