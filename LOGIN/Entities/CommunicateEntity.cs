using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LOGIN.Entities
{
    [Table("communicate", Schema = "post")]
    public class CommunicateEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [StringLength(100)]

        [Required]
        [Column("tittle")]
        public string Tittle { get; set; }

        [Required]
        [Column("date")]
        public DateTime Date { get; set; }

       
        [StringLength(20)]
        [Column("type_statement")]
        public string? Type_Statement { get; set; }

        [Required]
        [StringLength(8000)]
        [Column("content")]
        public string Content { get; set; }

        [Column("user_id")]
        public string User_Id {  get; set; }

        [ForeignKey(nameof(User_Id))]
        public virtual UserEntity User { get; set; }
    }
}
