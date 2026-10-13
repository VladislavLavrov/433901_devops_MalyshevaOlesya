
using Calculator.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Calculator.Controllers
{
    public enum Operation { Add, Subtract, Multiply, Divide }

    public class CalculatorController : Controller
    {
        private readonly CalculatorContext _context;

        public CalculatorController(CalculatorContext context)
        {
            _context = context;
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
        public IActionResult Calculate(double num1, double num2, Operation operation)
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

            _context.DataInputVariants.Add(new DataInputVariant
            {
                Operand_1 = num1,
                Operand_2 = num2,
                Type_operation = operation.ToString(),
                Result = result,
                CreatedAt = DateTime.Now
            });

            _context.SaveChanges();

            ViewBag.Result = result;
            LoadHistory();

            return View("Index");
        }
    }
}
