using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSW.Model
{

    [Table("Medicine")]
    public class Medicine
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("dose")]
        public int Dose { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        public Medicine()
        {
            Id = "medicine_" + Guid.NewGuid();
            Quantity = 0;
            Name = "Empty";
            Dose = 0;
        }

        public Medicine(Medicine newMedicine)
        {
            Id = "medicine_" + Guid.NewGuid();
            Name = newMedicine.Name;
            Dose = newMedicine.Dose;
            Quantity = newMedicine.Quantity;
        }
    }
}
