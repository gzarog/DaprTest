using InventoryService.Interfaces;
using InventoryService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddDapr();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IInventoryRepository, StateInventoryRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCloudEvents();

app.UseAuthorization();
app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapSubscribeHandler();
    }
);

var inventory = app.Services.GetService<IInventoryRepository>();
inventory.InitDataToState();

app.Run();
