
using Microsoft.AspNetCore.Http;

namespace Domain.Models
{
    public class DocumentModel
    {
        public int ID { get; set; }
        public string Designation { get; set; }
        public IFormFile File { get; set; }
        public int ClientID { get; set; }

        public string NomFichier { get; set; }
    }
}
