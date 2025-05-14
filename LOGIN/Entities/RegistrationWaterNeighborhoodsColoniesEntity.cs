using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOGIN.Entities
{
    [Table("registrationWaterNeighborhoodsColonies", Schema = "calendar")]
    public class RegistrationWaterNeighborhoodsColoniesEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("registrationWaterId")]
        public Guid RegistrationWaterId { get; set; }
        [ForeignKey(nameof(RegistrationWaterId))]
        public virtual RegistrationWaterEntity RegistrationWater { get; set; }

        [Column("neighborhoodColoniesId")]
        public Guid NeighborhoodColoniesId { get; set; }
        [ForeignKey(nameof(NeighborhoodColoniesId))]
        public virtual NeighborhoodsColoniesEntity NeighborhoodsColonies { get; set; }
    }
}
