using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
#nullable disable
    public class DocumentINDH
    {
        public int DocumentINDHID { get; set; }
        public string Designation { get; set; }
        public string NomFichier { get; set; }

        public int ClientID { get; set; }
        public ClientFinance Client { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

    }
}
