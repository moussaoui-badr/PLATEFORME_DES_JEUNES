using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class FormationModel
    {
        public int FormationID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DateFormation { get; set; }
        public int DureeFormation { get; set; }
        public string Theme { get; set; }
        public int nbreCandidat { get; set; }
    }
}
