using ICPC_WebSite_Backend.Configurations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

ConfigProvider.Configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(o => o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureDatabase();
builder.Services.RegisterRepos();
builder.Services.RegisterAuth();
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
    o.TokenLifespan = TimeSpan.FromHours(1));
builder.Services.RegisterAuthorization();
var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
     await scope.ServiceProvider.CreateRoles();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true) {//true to work on any environment 
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
