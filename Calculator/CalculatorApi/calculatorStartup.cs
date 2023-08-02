using Calculator.CalculatorLogic;
using Calculator.Controller;
using Calculator.middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.Configure<RateLimiterOptions>(options =>
    options.Limit = builder.Configuration.GetValue<int>("Settings:RateLimit"));

builder.Services.Configure<CalculatorOptions>(options =>
    options.AllowedOperators = builder.Configuration.GetValue<string>("Settings:AllowedSymbols"));



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseRateLimiter();
app.UseSymbolsCheck();
app.MapControllers();
app.UseHttpsRedirection();
app.UseAuthorization();

app.Run();