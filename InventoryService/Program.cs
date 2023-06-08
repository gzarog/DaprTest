using InventoryService.Interfaces;
using InventoryService.Repositories;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddDapr();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IInventoryRepository, StateInventoryRepository>();


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Seq("http://seq_service:5341") 
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddOpenTelemetryTracing(builder =>
{
    builder
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("InventoryService"))
    .AddAspNetCoreInstrumentation()
    .AddHttpClientInstrumentation()
    
    .AddZipkinExporter(o =>
        {
            o.Endpoint = new Uri("http://zipkin_service:9411/api/v2/spans");
        });
});

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
