using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{

    public class Client
    {
        public int? ClientID { get; set; }
        public string? Commentaire { get; set; }

        [Display(Name = "Profil")]
        public string? ImageUrl { get; set; }
        public Sexe? Sexe { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? DateNaissance { get; set; }
        public SituationFamilial? SituationFamilial { get; set; }
        public string? Adresse { get; set; }
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? CIN { get; set; }
        public string? DecouvertePlateForme { get; set; }
        public bool? Oriente { get; set; }
        public Orientation? Orientation { get; set; }
        public Statut? Statut { get; set; }
        public bool? Public { get; set; }
        public DateTime? Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? DateAderation { get; set; }
        public ICollection<InscriptionFormation>? InscriptionFormation { get; set; }
        public ICollection<Diplome>? Diplomes { get; set; }
        public ICollection<Document>? Documents { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
    }

    public enum SituationFamilial
    {
        Marié,
        Celibataire
    }
    public enum Statut
    {
        PorteurDeProjet,
        ChercheurEmploit
    }

    public enum Orientation
    {
        Orienté,
        NonOrienté,
        Refusé,
        Financé
    }

    public enum Sexe
    {
        Homme,
        Femme
    }

}
