using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Diplome
    {
        public int DiplomeID { get; set; }
        public string DiplomeName { get; set; }
        public string Branche { get; set; }
        public string InstitutName { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateObtentionDiplome { get; set; }

        public int ClientID { get; set; }
        public Client Client { get; set; }
    }
}
