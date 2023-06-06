namespace Domain.Entities
{
    public class RabbitMqConnectionData
    {
        public string HostName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string VirtualHost { get; set; } = default!;
        public string ChannelName { get; set; } = default!;
    }
}