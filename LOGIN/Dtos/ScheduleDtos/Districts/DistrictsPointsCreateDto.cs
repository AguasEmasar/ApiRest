using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.ScheduleDtos.Districts
{
    public class DistrictsPointsCreateDto
    {
        [Required]
        public Guid NeighborhoodsColoniesId { get; set; }

        [Display(Name = "latitude")]
        [Required(ErrorMessage = "La {0} es requerida")]
        public decimal Latitude { get; set; }

        [Display(Name = "longitude")]
        [Required(ErrorMessage = "La {0} es requerida")]
        public decimal Longitude { get; set; }
    }
}
