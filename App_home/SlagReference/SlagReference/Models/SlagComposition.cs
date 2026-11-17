using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace SlagReference.Models;
public class SlagComposition {
 public int Id {get;set;}
 [Required, StringLength(120), Display(Name="Название состава")] public string Name {get;set;}="";
 [Range(0,100), Column(TypeName="decimal(6,2)"), Display(Name="CaO, %")] public decimal CaO {get;set;}
 [Range(0,100), Column(TypeName="decimal(6,2)"), Display(Name="MgO, %")] public decimal MgO {get;set;}
 [Range(0,100), Column(TypeName="decimal(6,2)"), Display(Name="Al₂O₃, %")] public decimal Al2O3 {get;set;}
 [Range(0,100), Column(TypeName="decimal(6,2)"), Display(Name="SiO₂, %")] public decimal SiO2 {get;set;}
 [StringLength(1000), Display(Name="Примечание")] public string? Notes {get;set;}
 [NotMapped] public decimal Total => CaO+MgO+Al2O3+SiO2;
 public ICollection<TemperatureCharacteristic> Temperatures {get;set;} = new List<TemperatureCharacteristic>();
}
