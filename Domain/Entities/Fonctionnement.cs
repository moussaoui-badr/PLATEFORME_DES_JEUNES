namespace Domain.Entities
{
#nullable disable
    public class Fonctionnement
    {
        public int FonctionnementID { get; set; }
        public int? ChapitreId { get; set; }
        public Chapitre? Chapitre { get; set; }
        public string? NuméroCheque { get; set; }
        public DateTime? Date { get; set; }
        public string? Beneficiaire { get; set; }
        public double? Montant { get; set; }
        public DateTime? DateCreation { get; set; } = DateTime.Now;

    }
}
