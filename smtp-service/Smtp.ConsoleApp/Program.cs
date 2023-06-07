using System;
using System.IO;
using System.Text;
using System.Text.Json;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Smtp.ConsoleApp.Models;
using Smtp.ConsoleApp.Services;
using Smtp.ConsoleApp.Services.Contracts;

internal class Program
{
    static void Main(string[] args)
    {
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider();
        var pdfGenerator = serviceProvider.GetService<IPdfGenerator>()!;
        var emailSender = serviceProvider.GetService<IEmailSender>()!;
        var config = serviceProvider.GetService<IConfiguration>()!;

        Console.WriteLine("Listening for messages...");

        var factory = new ConnectionFactory()
        {
            HostName = config["RabbitMqConfiguration:HostName"],
            UserName = config["RabbitMqConfiguration:UserName"],
            Password = config["RabbitMqConfiguration:Password"],
            VirtualHost = config["RabbitMqConfiguration:VirtualHost"],
        };

        var connection = factory.CreateConnection();

        using var channel = connection.CreateModel();
        channel.QueueDeclare("email-sending", durable: true, exclusive: false);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, eventArgs) => 
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine("Message is received. " + message);

            var emailData = JsonSerializer.Deserialize<EmailInfo>(message);

            var pdfInBytes = pdfGenerator.CreatePDF(emailData.Book);
            
            emailSender.SendEmail(emailData.EmailTo, pdfInBytes);
        };

        channel.BasicConsume("email-sending", true, consumer);
        
        Console.ReadKey();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();
        
        services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        services.AddSingleton<IConfiguration>(configuration);
        services.AddScoped<IEmailSender, EmailSender>();
        services.AddScoped<IPdfGenerator, PdfGenerator>();
        services.AddScoped<ITemplateGenerator, TemplateGenerator>();
    }
}
