using System.ComponentModel.DataAnnotations.Schema;

namespace KhalfiElection.Models.Entities
{
    public class Personne
    {
        public int PersonneId { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? CIN { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string? Adresse { get; set; }
        public string? GSM { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public int? SecteurId { get; set; }
        public Secteur? Secteur { get; set; }

        public int? RelationParenteId { get; set; }
        public TypeRelationParente? RelationParente { get; set; }

        public int? FamilleId { get; set; }
        public Famille? Famille { get; set; }

        public bool? IsResponsable { get; set; }

        public int? PivotId { get; set; }
        public Personne? Pivot { get; set; }
    }

}
