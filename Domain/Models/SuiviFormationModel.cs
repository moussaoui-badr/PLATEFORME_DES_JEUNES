using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class SuiviFormationModel
    {
        public int SuiviFormationID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime DateFormation { get; set; }
        public int DureeFormation { get; set; }
        public string Theme { get; set; }
    }
}
