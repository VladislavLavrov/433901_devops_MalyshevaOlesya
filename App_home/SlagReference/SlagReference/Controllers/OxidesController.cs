using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SlagReference.Data;
namespace SlagReference.Controllers;
public class OxidesController(AppDbContext db):Controller {
 public async Task<IActionResult> Index()=>View(await db.OxideReferences.OrderBy(x=>x.Formula).ToListAsync());
}
