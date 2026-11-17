using SlagReference.Models;
namespace SlagReference.Data;
public static class Seed {
 public static async Task InitializeAsync(AppDbContext db) {
  if(!db.OxideReferences.Any()) db.OxideReferences.AddRange(
   new(){Name="Оксид кальция",Formula="CaO",Description="Компонент оксидной системы доменного шлака."},
   new(){Name="Оксид магния",Formula="MgO",Description="Компонент оксидной системы доменного шлака."},
   new(){Name="Оксид алюминия",Formula="Al₂O₃",Description="Компонент оксидной системы доменного шлака."},
   new(){Name="Диоксид кремния",Formula="SiO₂",Description="Компонент оксидной системы доменного шлака."});
  if(!db.SlagCompositions.Any()) {
   db.SlagCompositions.AddRange(
    new(){Name="Демонстрационный состав 1",CaO=40,MgO=10,Al2O3=15,SiO2=35,Notes="Тестовая запись, не нормативные данные."},
    new(){Name="Демонстрационный состав 2",CaO=42,MgO=8,Al2O3=12,SiO2=38,Notes="Тестовая запись, не нормативные данные."});
   await db.SaveChangesAsync();
   var id=db.SlagCompositions.OrderBy(x=>x.Id).First().Id;
   db.TemperatureCharacteristics.AddRange(
    new(){SlagCompositionId=id,TemperatureC=1400,Description="Демонстрационная запись. Заменить данными из проверенного источника.",Source="Тестовые данные"},
    new(){SlagCompositionId=id,TemperatureC=1450,Description="Демонстрационная запись. Заменить данными из проверенного источника.",Source="Тестовые данные"});
  }
  await db.SaveChangesAsync();
 }
}
