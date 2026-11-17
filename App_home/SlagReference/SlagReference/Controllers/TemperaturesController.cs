using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SlagReference.Data;
using SlagReference.Models;
namespace SlagReference.Controllers;
public class TemperaturesController(AppDbContext db):Controller {
 public async Task<IActionResult> Index()=>View(await db.TemperatureCharacteristics.Include(x=>x.SlagComposition).OrderBy(x=>x.TemperatureC).ToListAsync());
 public async Task<IActionResult> Create(){await Load();return View(new TemperatureCharacteristic());}
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(TemperatureCharacteristic x) {
  if(!ModelState.IsValid){await Load();return View(x);} db.Add(x);await db.SaveChangesAsync();return RedirectToAction(nameof(Index));
 }
 public async Task<IActionResult> Delete(int id){var x=await db.TemperatureCharacteristics.Include(x=>x.SlagComposition).FirstOrDefaultAsync(x=>x.Id==id);return x==null?NotFound():View(x);}
 [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id){var x=await db.TemperatureCharacteristics.FindAsync(id);if(x!=null){db.Remove(x);await db.SaveChangesAsync();}return RedirectToAction(nameof(Index));}
 async Task Load()=>ViewBag.Compositions=new SelectList(await db.SlagCompositions.OrderBy(x=>x.Name).ToListAsync(),"Id","Name");
}
