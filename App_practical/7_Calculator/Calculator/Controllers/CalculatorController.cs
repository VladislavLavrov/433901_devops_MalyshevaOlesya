using Calculator.Data;
using Calculator.Models;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Controllers
{
    public enum Operation { Add, Subtract, Multiply, Divide }

    public class CalculatorController : Controller
    {
        private CalculatorContext _context;

        public CalculatorController(CalculatorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.History = _context.DataInputVariants
                .OrderByDescending(x => x.ID_DataInputVariant)
                .Take(10)
                .ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate(double num1, double num2, Operation operation)
        {
            double result = 0;
            switch (operation)
            {
                case Operation.Add:
                    result = num1 + num2;
                    break;
                case Operation.Subtract:
                    result = num1 - num2;
                    break;
                case Operation.Multiply:
                    result = num1 * num2;
                    break;
                case Operation.Divide:
                    if (num2 == 0)
                    {
                        ModelState.AddModelError("", "На ноль делить нельзя.");
                        return View("Index");
                    }
                    result = num1 / num2;
                    break;
            }
            ViewBag.Result = result;

            DataInputVariant dataInputVariant = new DataInputVariant();
            dataInputVariant.Operand_1 = num1.ToString();
            dataInputVariant.Operand_2 = num2.ToString();
            dataInputVariant.Type_operation = operation.ToString();
            dataInputVariant.Result = result.ToString();

            _context.DataInputVariants.Add(dataInputVariant);
            _context.SaveChanges();

            return View("Index");
        }
    }
}
