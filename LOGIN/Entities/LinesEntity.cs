using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOGIN.Entities
{
    [Table("lines", Schema = "calendar")]
    public class LinesEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(30)]
        public string Name { get; set; }

        [Column("neighborhoodsColonies_id")]
        public Guid NeighborhoodsColoniesId { get; set; }
        
        [ForeignKey(nameof(NeighborhoodsColoniesId))]
        public virtual NeighborhoodsColoniesEntity NeighborhoodsColonies { get; set; }
    }
}
