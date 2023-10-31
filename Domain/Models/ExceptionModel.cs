using Domain.Entities;

namespace Domain.Models
{
    public class ExceptionModel
    {
        public Client Client { get; set; }
        public string Exception1 { get; set; }
        public string Exception2 { get; set; }
        public string Exception3 { get; set; }
        public string Exception4 { get; set; }
        public string Exception5 { get; set; }
        public Exception exception { get; set; }
    }
}
