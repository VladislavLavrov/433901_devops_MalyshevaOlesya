using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlagReference.Data;
using SlagReference.Models;
namespace SlagReference.Controllers;
public class CompositionsController(AppDbContext db):Controller {
 public async Task<IActionResult> Index(string? search) {
  var q=db.SlagCompositions.AsQueryable();
  if(!string.IsNullOrWhiteSpace(search)) q=q.Where(x=>x.Name.Contains(search));
  ViewBag.Search=search; return View(await q.OrderBy(x=>x.Name).ToListAsync());
 }
 public async Task<IActionResult> Details(int id) {
  var x=await db.SlagCompositions.Include(x=>x.Temperatures).FirstOrDefaultAsync(x=>x.Id==id);
  return x==null?NotFound():View(x);
 }
 public IActionResult Create()=>View(new SlagComposition());
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Create(SlagComposition x) {
  CheckTotal(x); if(!ModelState.IsValid)return View(x);
  db.Add(x); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
 }
 public async Task<IActionResult> Edit(int id) { var x=await db.SlagCompositions.FindAsync(id); return x==null?NotFound():View(x); }
 [HttpPost,ValidateAntiForgeryToken] public async Task<IActionResult> Edit(int id,SlagComposition x) {
  if(id!=x.Id)return BadRequest(); CheckTotal(x); if(!ModelState.IsValid)return View(x);
  db.Update(x); await db.SaveChangesAsync(); return RedirectToAction(nameof(Index));
 }
 public async Task<IActionResult> Delete(int id) { var x=await db.SlagCompositions.FindAsync(id); return x==null?NotFound():View(x); }
 [HttpPost,ActionName("Delete"),ValidateAntiForgeryToken] public async Task<IActionResult> DeleteConfirmed(int id) {
  var x=await db.SlagCompositions.FindAsync(id); if(x!=null){db.Remove(x);await db.SaveChangesAsync();}
  return RedirectToAction(nameof(Index));
 }
 void CheckTotal(SlagComposition x) { if(x.Total!=100m)ModelState.AddModelError("",$"Сумма компонентов должна быть 100%. Сейчас: {x.Total:0.##}%."); }
}
