using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOGIN.Entities
{
    [Table("report", Schema = "reports")]
    public class ReportEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("state_id")]
        public Guid StateId { get; set; }

        [ForeignKey(nameof(StateId))]
        public virtual StateEntity State { get; set; }

        [Required]
        [Column("key")]
        public string Key { get; set; }
         
        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("dni")]
        public string DNI { get; set; }

        [Required]
        [Column("cellphone")]
        public string Cellphone { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

        [Required]
        [Column("report")]
        public string Report { get; set; }

        [Required]
        [Column("direction")]
        public string Direction { get; set; }

        [Required]
        [Column("observation")]
        public string Observation { get; set; }

        [Required]
        [Column("publicids")]
        public List<string> PublicIds { get; set; }

        [Required]
        [Column("urls")]
        public List<string> Urls { get; set; }

    }
}
