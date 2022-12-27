using System.Text.Json.Serialization;
using server.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IOnspringService, OnspringService>();
builder.Services.AddScoped<IFormulaService, JintFormulaService>();

builder.Services.AddCors(p => p.AddPolicy("corsPolicy", build =>
{
  build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseCors("corsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
