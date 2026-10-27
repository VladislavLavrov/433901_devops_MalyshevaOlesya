using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calculator.Data
{
    [Table("DataInputVariants")]
    public class DataInputVariant
    {
        [Key]
        public int ID_DataInputVariant { get; set; }

        public double Operand_1 { get; set; }

        public double Operand_2 { get; set; }

        public int Type_operation { get; set; }

        public string? Result { get; set; }
    }
}