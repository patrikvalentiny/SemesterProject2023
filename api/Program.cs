using api;
using api.Middleware;
using infrastructure;
using infrastructure.DataModels;
using infrastructure.Repositories;
using Serilog;
using service.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

Log.Information("Starting web application");

builder.Host.UseSerilog();
builder.Services.AddNpgsqlDataSource(
    Utilities.FormatConnectionString(
        Environment.GetEnvironmentVariable("ASPNETCORE_ConnectionStrings__WebApiDatabase")!),
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
builder.Services.AddJwtService();
builder.Services.AddSwaggerGenWithBearerJwt();
builder.Services.AddSingleton<IRepository<User>, UserRepository>();
builder.Services.AddSingleton<IRepository<UserDetails>, UserDetailsRepository>();
builder.Services.AddSingleton<UserDetailsService>();
builder.Services.AddSingleton<WeightService>();
builder.Services.AddSingleton<WeightRepository>();
builder.Services.AddSingleton<PasswordRepository>();
builder.Services.AddSingleton<AccountService>();

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

var allowedOrigins = app.Environment.IsDevelopment()
    ? new List<string> { "http://localhost:4200", "http://localhost:5000" }
    : new List<string> { "https://weighttrackerpatval.azurewebsites.net", "https://semesterproject2023-7161a.web.app" };
app.UseCors(policyBuilder => policyBuilder.SetIsOriginAllowed(origin => allowedOrigins.Contains(origin))
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseSecurityHeaders();

app.UseAuthorization();


app.MapControllers();
app.UseMiddleware<JwtBearerHandler>();

app.Run();