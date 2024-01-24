using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
#nullable disable
    public class ClientFinance
    {
        public int ClientFinanceID { get; set; }
        public string Commentaire { get; set; }

        [Display(Name = "Profil")]
        public string ImageUrl { get; set; }
        public SexeC Sexe { get; set; }
        public string Nom { get; set; }

        public string Prenom { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string CIN { get; set; }
        public DateTime Created { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? DateAderation { get; set; }
        public ICollection<DocumentINDH> Documents { get; set; }
        public PlateformeGestionnaire PlateformeGestionnaire { get; set; }
        [NotMapped]
        public IFormFile File { get; set; }

        public double MontantProjet { get; set; }
        public double MontantApportPersonnel { get; set; }
        public double MontantINDH { get; set; }

        [NotMapped]
        public int Etat { get; set; }
        [NotMapped]
        public int? Cloture_EnCours { get; set; }
        public DateTime? Modified { get; set; }
        public ICollection<INDH> INDHS { get; set; }
        public bool ApportPersonnelConfirme { get; set; }
    }

    public enum SexeC
    {
        Homme,
        Femme
    }

    public enum PlateformeGestionnaire
    {
        HayMohammadi,
        AinSebaa,
        RocheNoir
    }

}
