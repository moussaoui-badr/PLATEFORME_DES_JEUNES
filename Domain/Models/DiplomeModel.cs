using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class DiplomeModel
    {
        public int ID { get; set; }
        public string DiplomeName { get; set; }
        public string Branche { get; set; }
        public string InstitutName { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DateObtentionDiplome { get; set; }
        public int ClientID { get; set; }
    }
}
