using MassTransit;
using Shopping.Catalog.Entities;
using Shopping.Common.MassTransit;
using Shopping.Common.MongoDB;
using Shopping.Common.Settings;


var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddMongo(builder.Configuration)
    .AddMongRepo<Item>("items")
    .AddMassTransitWithRabbitMq();

builder.Services.AddControllers(options=>
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
