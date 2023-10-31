using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Chapitre
    {
        public int ChapitreID { get; set; }
        public string ChapitreTitle { get; set; }
        public double MontantTotale { get; set; }
        [NotMapped]
        public double MontantRestant { get; set; }
        public List<Fonctionnement> Fonctionnements { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;
    }
}
