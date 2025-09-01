using Microsoft.EntityFrameworkCore;
using Vwr.Domain;
using Vwr.Domain.Entities;
using Vwr.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Configure the HTTP request pipeline.


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


builder.Services.AddAppDb(builder.Configuration.GetConnectionString("pg"));
// Options for EVM (bound from appsettings.json)
builder.Services.Configure<EvmOptions>(builder.Configuration.GetSection("evm"));


builder.Services.AddScoped<ISignatureVerifier, EthereumSignatureVerifier>();
builder.Services.AddScoped<IReceiptPublisher, EvmReceiptPublisher>();

var corsOrigin = "http://localhost:5173";
builder.Services.AddCors(o =>
{
    o.AddPolicy("vwr", p => p
        .WithOrigins(corsOrigin)
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDb>();
    await db.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseCors("vwr");
app.UseRouting();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();