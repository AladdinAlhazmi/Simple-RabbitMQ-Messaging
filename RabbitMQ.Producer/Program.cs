using System.Text;
using RabbitMQ.Client;


// This is a placeholder for the RabbitMQ Producer application.
var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(
    queue: "message",
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

for (int i = 0; i < 10; i++)
{
    var message = $"{DateTime.UtcNow} - Message {i}";
    var body = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(
        exchange: string.Empty,
        routingKey: "message",
        mandatory: true,
        basicProperties: new BasicProperties { Persistent = true},
        body: body);
    Console.WriteLine($"Sent: {message}");
    await Task.Delay(1000); // Simulate some delay between messages
}