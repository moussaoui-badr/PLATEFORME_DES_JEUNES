using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
#nullable disable
    public class Document
    {
        public int DocumentID { get; set; }
        public string Designation { get; set; }
        public string NomFichier { get; set; }

        public int ClientID { get; set; }
        public Client Client { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

    }
}
