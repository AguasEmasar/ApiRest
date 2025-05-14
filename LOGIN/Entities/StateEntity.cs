
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOGIN.Entities
{

    [Table("state", Schema = "reports")]
    public class StateEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid(); // Genera un guid nuevo

        [Required]
        [Column("name")]
        public string Name { get; set; }

    }
}
