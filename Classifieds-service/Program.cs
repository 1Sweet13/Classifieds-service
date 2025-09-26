using Classifieds_service.Data;
using Classifieds_service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PromotionData>();




var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();
}





app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
