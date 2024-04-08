using MassTransit;

var builder = WebApplication.CreateBuilder(args);
var defaultBuilder = Host.CreateDefaultBuilder(args);

// Add services to the container.
//MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        //config.UseHealthCheck(provider);
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    }));
});

//new way
//defaultBuilder.ConfigureServices((hostContext, services) =>
//{
//    services.AddMassTransit(cfg =>
//    {
//        cfg.UsingRabbitMq((context, cfg) =>
//        {
//            cfg.Host("rabbitmq://localhost/", h =>
//            {
//                h.Username("guest");
//                h.Password("guest");
//            });
//        });
//    });

//    services.AddMassTransitHostedService();
//});

//builder.Services.AddMassTransitHostedService();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
