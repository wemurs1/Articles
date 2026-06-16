using System.ComponentModel.DataAnnotations;

namespace Blocks.Messaging;

public class RabbitMqOptions
{
    [Required]
    public string Host { get; set; } = "localhost"; // default to localhost

    [Required]
    public string UserName { get; set; } = "guest"; // default RabbitMq

    [Required]
    public string Password { get; set; } = "guest"; // default RabbitMq

    [Required]
    public string VirtualHost { get; set; } = "/"; // Default RabbitMq
}
