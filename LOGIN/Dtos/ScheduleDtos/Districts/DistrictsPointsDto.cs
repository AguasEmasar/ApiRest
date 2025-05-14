namespace LOGIN.Dtos.ScheduleDtos.Districts
{
    public class DistrictsPointsDto
    {
        public Guid Id { get; set; }
        public Guid NeighborhoodsColoniesId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
