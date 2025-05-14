using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LOGIN.Entities
{
    [Table("neighborhoodsColonies", Schema = "calendar")]
    public class NeighborhoodsColoniesEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Required]
        [Column("name")]
        [StringLength(30)]
        public string Name { get; set; }

        [Column("block_id")]
        public Guid BlockId { get; set; }

        [ForeignKey(nameof(BlockId))]
        public virtual BlocksEntity Block { get; set; }
    }
}
