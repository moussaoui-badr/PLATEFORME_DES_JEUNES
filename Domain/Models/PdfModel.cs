
using Domain.Entities;

namespace Domain.Models
{
    public class PdfModel
    {
        public Client Client { get; set; }
        public List<Diplome> Diplomes { get; set; }
        public List<Formation> Formations { get; set; }
    }
}
