using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOGIN.Entities
{
    [Table("districtsPoints", Schema = "calendar")]
    public class DistrictsPointsEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("neighborhoodsColonies_id")]
        public Guid NeighborhoodsColoniesId { get; set; }

        [ForeignKey(nameof(NeighborhoodsColoniesId))]
        public virtual NeighborhoodsColoniesEntity NeighborhoodsColonies { get; set; }

        [Required]
        [Column("latitude")]
        public string Latitude { get; set; }

        [Required]
        [Column("longitude")]
        public string Longitude { get; set; }
    }
}
