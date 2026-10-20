
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace Calculator.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly ILogger<KafkaConsumerService> _logger;
        private readonly IConfiguration _configuration;

        public KafkaConsumerService(
            ILogger<KafkaConsumerService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _configuration["Kafka:BootstrapServers"],
                GroupId = "calculator-consumer-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = true
            };

            using var consumer = new ConsumerBuilder<string, string>(config).Build();

            consumer.Subscribe("malysheva");

            _logger.LogInformation("Kafka consumer запущен");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var result = consumer.Consume(stoppingToken);
                    _logger.LogInformation(
                        "Kafka: получено сообщение: {Message}",
                        result.Message.Value);

                    await Task.Yield();
                }
            }
            catch (OperationCanceledException)
            {
                // Сервис останавливается
            }
            finally
            {
                consumer.Close();
            }
        }
    }
}
