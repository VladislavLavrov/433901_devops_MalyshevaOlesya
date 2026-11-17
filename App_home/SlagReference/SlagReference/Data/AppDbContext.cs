using Microsoft.EntityFrameworkCore;
using SlagReference.Models;
namespace SlagReference.Data;
public class AppDbContext(DbContextOptions<AppDbContext> options):DbContext(options) {
 public DbSet<SlagComposition> SlagCompositions=>Set<SlagComposition>();
 public DbSet<TemperatureCharacteristic> TemperatureCharacteristics=>Set<TemperatureCharacteristic>();
 public DbSet<OxideReference> OxideReferences=>Set<OxideReference>();
 protected override void OnModelCreating(ModelBuilder b) {
  b.Entity<SlagComposition>().HasMany(x=>x.Temperatures).WithOne(x=>x.SlagComposition).HasForeignKey(x=>x.SlagCompositionId).OnDelete(DeleteBehavior.Cascade);
 }
}
