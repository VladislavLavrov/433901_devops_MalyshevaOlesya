using System.ComponentModel.DataAnnotations;
namespace SlagReference.Models;
public class TemperatureCharacteristic {
 public int Id {get;set;}
 [Required, Display(Name="Состав")] public int SlagCompositionId {get;set;}
 public SlagComposition? SlagComposition {get;set;}
 [Range(0,2500), Display(Name="Температура, °C")] public int TemperatureC {get;set;}
 [StringLength(1000), Display(Name="Справочная характеристика")] public string? Description {get;set;}
 [StringLength(300), Display(Name="Источник")] public string? Source {get;set;}
}
