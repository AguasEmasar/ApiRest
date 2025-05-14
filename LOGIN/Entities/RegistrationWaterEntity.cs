using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOGIN.Entities
{
    [Table("registrationWater", Schema = "calendar")]
    public class RegistrationWaterEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

        [Column("observations")]
        public string Observations { get; set; }

        // Relación de muchos a muchos con NeighborhoodsColoniesEntity
        public ICollection<RegistrationWaterNeighborhoodsColoniesEntity> RegistrationWaterNeighborhoodsColonies { get; set; }
    }
}