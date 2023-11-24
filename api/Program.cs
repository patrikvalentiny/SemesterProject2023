using api;
using api.Middleware;
using infrastructure;
using infrastructure.DataModels;
using infrastructure.Repositories;
using Serilog;
using service;
using service.Services;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    //.WriteTo.Console()
    //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .CreateLogger();

Log.Information("Starting web application");

builder.Host.UseSerilog();
builder.Services.AddNpgsqlDataSource(Utilities.FormatConnectionString(Environment.GetEnvironmentVariable("ASPNETCORE_ConnectionStrings__WebApiDatabase")!),
    dataSourceBuilder => dataSourceBuilder.EnableParameterLogging());
builder.Services.AddJwtService();
builder.Services.AddSwaggerGenWithBearerJWT();
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

var allowedOrigins = new List<string>{"http://localhost:4200"};
app.UseCors(policyBuilder => policyBuilder.SetIsOriginAllowed(origin => allowedOrigins.Contains(origin))
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseSecurityHeaders();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<JwtBearerHandler>();
    
app.Run();