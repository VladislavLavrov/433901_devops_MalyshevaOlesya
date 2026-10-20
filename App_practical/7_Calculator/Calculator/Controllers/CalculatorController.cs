
using Calculator.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Confluent.Kafka;
using System.Text.Json;

namespace Calculator.Controllers
{
    public enum Operation { Add, Subtract, Multiply, Divide }

    public class CalculatorController : Controller
    {
        private readonly CalculatorContext _context;
        private readonly IConfiguration _configuration;

        public CalculatorController(
            CalculatorContext context,
            IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                Type_operation = operation.ToString(),
                Result = result,
                CreatedAt = DateTime.Now
            };

            _context.DataInputVariants.Add(calculation);
            await _context.SaveChangesAsync();

            var message = new
            {
                Num1 = num1,
                Num2 = num2,
                Operation = operation.ToString(),
                Result = result,
                CreatedAt = calculation.CreatedAt
            };

            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers =
                        _configuration["Kafka:BootstrapServers"]
                };

                using var producer = new ProducerBuilder<string, string>(
                    config).Build();

                var json = JsonSerializer.Serialize(message);

                await producer.ProduceAsync(
                    "malysheva",
                    new Message<string, string>
                    {
                        Key = Guid.NewGuid().ToString(),
                        Value = json
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки в Kafka: {ex.Message}");
            }

            ViewBag.Result = result;
            LoadHistory();

            return View("Index");
        }
    }
}
