using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.ScheduleDtos.RegistrationWater
{
    public class RegistrationWaterNeighborhoodsColoniesDto
    {
        public Guid Id { get; set; }
        [Required]
        public Guid RegistrationWaterId { get; set; }

        [Required]
        public Guid NeighborhoodColoniesId { get; set; }
    }
}
