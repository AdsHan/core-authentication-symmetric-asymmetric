
using ASA.Consumer.API.Configuration;
using ASA.Core.Authentication;
using ASA.Core.DistributedCache;
using ASA.Core.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration();
builder.Services.AddSwaggerConfiguration();
builder.Services.AddDistributedCacheConfiguration(builder.Configuration);
builder.Services.AddSettingsConfiguration(builder.Configuration);
builder.Services.AddAuthenticationConfiguration(builder.Configuration);

var app = builder.Build();

app.UseApiConfiguration();
app.UseSwaggerConfiguration();
app.UseAuthenticationConfiguration();

app.Run();
