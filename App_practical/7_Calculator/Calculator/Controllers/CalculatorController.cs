
using Calculator.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Confluent.Kafka;
using System.Globalization;
using System.Text.Json;

namespace Calculator.Controllers
{
    public enum Operation { Add, Subtract, Multiply, Divide }

    public class CalculatorController : Controller
    {
        private readonly CalculatorContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(
            CalculatorContext context,
            IConfiguration configuration,
            ILogger<CalculatorController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        private void LoadHistory()
        {
            ViewBag.History = _context.DataInputVariants
                .AsNoTracking()
                .OrderByDescending(x => x.ID_DataInputVariant)
                .Take(20)
                .ToList();
        }

        [HttpGet]
        public IActionResult Index()
        {
            LoadHistory();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(
            double num1,
            double num2,
            Operation operation)
        {
            if (operation == Operation.Divide && num2 == 0)
            {
                ModelState.AddModelError("", "На ноль делить нельзя.");
                LoadHistory();
                return View("Index");
            }

            double result = operation switch
            {
                Operation.Add => num1 + num2,
                Operation.Subtract => num1 - num2,
                Operation.Multiply => num1 * num2,
                Operation.Divide => num1 / num2,
                _ => throw new ArgumentOutOfRangeException(nameof(operation))
            };

            var calculation = new DataInputVariant
            {
                Operand_1 = num1,
                Operand_2 = num2,
                Type_operation = (int)operation,
                Result = result.ToString(
        System.Globalization.CultureInfo.InvariantCulture)
            };

            _context.DataInputVariants.Add(calculation);
            await _context.SaveChangesAsync();

            var message = new
            {
                Num1 = num1,
                Num2 = num2,
                Operation = operation.ToString(),
                Result = result,
                Timestamp = DateTime.UtcNow
            };

            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers =
                        _configuration["Kafka:BootstrapServers"],
                    MessageTimeoutMs = 5000
                };

                using var producer =
                    new ProducerBuilder<string, string>(config).Build();

                var json = JsonSerializer.Serialize(message);
                var topic = _configuration["Kafka:Topic"] ?? "malysheva";

                var delivery = await producer.ProduceAsync(
                    topic,
                    new Message<string, string>
                    {
                        Key = Guid.NewGuid().ToString(),
                        Value = json
                    });

                _logger.LogInformation(
                    "Kafka: сообщение отправлено в {Topic}, partition {Partition}, offset {Offset}",
                    delivery.Topic,
                    delivery.Partition.Value,
                    delivery.Offset.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось отправить вычисление в Kafka");
                ViewBag.KafkaError = "Вычисление сохранено, но отправка в Kafka не удалась.";
            }

            ViewBag.Result = result;
            LoadHistory();
            return View("Index");
        }
    }
}
