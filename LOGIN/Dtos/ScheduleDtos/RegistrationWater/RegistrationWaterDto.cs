using System.ComponentModel.DataAnnotations;
using LOGIN.Dtos.ScheduleDtos.NeighborhoodsColonies;

namespace LOGIN.Dtos.ScheduleDtos.RegistrationWater
{
    public class RegistrationWaterDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Observations { get; set; }
        [Required]
        public List<NeighborhoodsColoniesDto> NeighborhoodColonies { get; set; } = new List<NeighborhoodsColoniesDto>();
    }
}
