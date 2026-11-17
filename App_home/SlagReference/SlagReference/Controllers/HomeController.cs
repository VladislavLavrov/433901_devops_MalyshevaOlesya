using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlagReference.Data;
namespace SlagReference.Controllers;
public class HomeController(AppDbContext db):Controller {
 public async Task<IActionResult> Index() {
  ViewBag.Compositions=await db.SlagCompositions.CountAsync();
  ViewBag.Temperatures=await db.TemperatureCharacteristics.CountAsync();
  ViewBag.Oxides=await db.OxideReferences.CountAsync();
  return View();
 }
}
