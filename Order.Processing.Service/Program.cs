using MassTransit;
using Order.Processing.Consumer.Service.Consumers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//MassTransit
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TicketConsumer>();
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        //config.UseHealthCheck(provider);
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        config.ReceiveEndpoint("ticketQueue", ep =>
        {
            ep.PrefetchCount = 16;
            ep.UseMessageRetry(r => r.Interval(2, 100));
            ep.ConfigureConsumer<TicketConsumer>(provider);
        });
    }));
});

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
