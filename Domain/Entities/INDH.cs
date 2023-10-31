using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class INDH
    {
        [Key]
        public int INDHID { get; set; }
        public string Materiel { get; set; }
        public double MontantAcquisition { get; set; }//Montant acquisition = PartIndh + ApportEnDhs + Autre
        public double PartIndh { get; set; }
        public double ApportEnDhs { get; set; }
        public double ApportEnAmenagement { get; set; }
        public double Autre { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public PlateformeGestionnaire? PlateformeGestionnaire { get; set; }
        public Etat Etat { get; set; }
        public int? ClientFinanceID { get; set; }
        public ClientFinance ClientFinance { get; set; }
    }

    public enum Etat
    {
        Acquis,
        EnCoursAcquisition,
        Amenagement
    }
}
