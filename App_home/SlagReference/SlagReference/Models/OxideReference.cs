using System.ComponentModel.DataAnnotations;
namespace SlagReference.Models;
public class OxideReference {
 public int Id {get;set;}
 [Required, StringLength(80)] public string Name {get;set;}="";
 [Required, StringLength(20)] public string Formula {get;set;}="";
 [StringLength(1000)] public string Description {get;set;}="";
}
