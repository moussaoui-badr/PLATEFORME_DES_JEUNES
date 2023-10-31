namespace Domain.Models
{
    public class ListeMaterielsUserModel
    {
        public int MaterielId { get; set; }
        public string Materiel { get; set; }
        public double Montant { get; set; }
        public double MontantDevis { get; set; }
        public int Etat { get; set; }
    }
}
