using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class AffectationClient
    {
        public int ClientID { get; set; }
        public string ImageUrl { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string CIN { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? DateAderation { get; set; }
        public int FormationID { get; set; }
        public bool AffectationValide { get; set; }
    }
}
