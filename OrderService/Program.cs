using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDaprClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder .Services.AddLogging();

builder.Services.AddOpenTelemetryTracing(builder =>
{
    builder
    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("OrderService"))
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

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapSubscribeHandler();
}
);
app.Run();
