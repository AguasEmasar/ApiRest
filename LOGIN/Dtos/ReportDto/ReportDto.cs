using LOGIN.Dtos.States;

namespace LOGIN.Dtos.ReportDto
{
    public class ReportDto
    {

        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string DNI { get; set; }
        public string Cellphone { get; set; }
        public DateTime Date { get; set; }
        public string Report { get; set; }
        public string Direction { get; set; }
        public string Observation { get; set; }
        public List<string> PublicIds { get; set; }
        public List<string> Urls { get; set; }
        public StateDto State { get; set; }

    }
}