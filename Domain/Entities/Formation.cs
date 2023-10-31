using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Formation
    {
        public int FormationID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime DateFormation { get; set; }
        [Display(Name = "Durée")]
        public int DureeFormation { get; set; }
        public string Theme { get; set; }
        public string Animateur { get; set; }
        public ICollection<InscriptionFormation> InscriptionFormation { get; set; }
    }
}
