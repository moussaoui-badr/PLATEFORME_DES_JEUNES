using Domain.Entities;

namespace Domain.Models
{
    public class INDH_FiltreModel
    {

        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string CIN { get; set; }
        public string PlateformeGestionnaire { get; set; }
        public bool ApportPersonnel { get; set; }
        public int? ClientFinanceID { get; set; }
        public string ImageURL { get; set; }
        public double MontantProjet { get; set; }
        public double MontantTotalDevis { get; set; }
        public double MontantApportPersonnel { get; set; }
        public double MontantINDH { get; set; }
        public List<INDH> INDHS { get; set; }
    }
}
