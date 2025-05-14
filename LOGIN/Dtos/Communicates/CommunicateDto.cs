using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.Communicates
{
    public class CommunicateDto
    {
        public Guid Id { get; set; }

        public string Tittle { get; set; }

        public DateTime Date { get; set; }

        public string Type_Statement { get; set; }

        public string Content { get; set; }

    }
}
