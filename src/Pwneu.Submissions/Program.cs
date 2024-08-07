using MassTransit;
using Pwneu.Shared.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var assembly = typeof(Program).Assembly;

// RabbitMQ
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumers(assembly);
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration[Consts.MessageBrokerHost]!), h =>
        {
            h.Username(builder.Configuration[Consts.MessageBrokerUsername]!);
            h.Password(builder.Configuration[Consts.MessageBrokerPassword]!);
        });
        configurator.ConfigureEndpoints(context);
    });
});

// Assembly scanning of Mediator
builder.Services.AddMediatR(config => config.RegisterServicesFromAssembly(assembly));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();