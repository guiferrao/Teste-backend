using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Teste.Api.Data;
using Teste.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TesteDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ViaCepService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Teste");
    });
}
    
app.MapControllers();

app.UseHttpsRedirection();

app.Run();

