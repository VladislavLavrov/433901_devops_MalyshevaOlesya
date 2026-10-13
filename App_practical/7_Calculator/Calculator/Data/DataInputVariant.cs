
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calculator.Data
{
    [Table("Calculations")]
    public class DataInputVariant
    {
        [Key]
        [Column("Id")]
        public int ID_DataInputVariant { get; set; }

        [Column("FirstNumber")]
        public double Operand_1 { get; set; }

        [Column("SecondNumber")]
        public double Operand_2 { get; set; }

        [Column("Operation")]
        public string Type_operation { get; set; } = "";

        [Column("Result")]
        public double Result { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; }
    }
}
