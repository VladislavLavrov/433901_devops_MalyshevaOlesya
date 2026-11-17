using Microsoft.EntityFrameworkCore;
using SlagReference.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var cs = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Missing DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(o => o.UseMySql(cs, ServerVersion.AutoDetect(cs)));
var app = builder.Build();
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Home/Error"); app.UseHsts(); }
app.UseHttpsRedirection(); app.UseStaticFiles(); app.UseRouting(); app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
using (var scope = app.Services.CreateScope()) {
 var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
 await db.Database.EnsureCreatedAsync(); await Seed.InitializeAsync(db);
}
app.Run();
