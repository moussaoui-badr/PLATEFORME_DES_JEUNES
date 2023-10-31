using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{

    //public enum SituationFamilial
    //{
    //    Marié,
    //    Celibataire
    //}
    //public enum Statut
    //{
    //    PorteurDeProjet,
    //    ChercheurEmploit
    //}

    //public enum Sexe
    //{
    //    Home,
    //    Femme
    //}

    public class ClientPublic
    {
        public int ClientPublicID { get; set; }
        [Display(Name = "Profil")]
        public string ImageUrl { get; set; }
        public Sexe Sexe { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }
        public SituationFamilial SituationFamilial { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public DateTime Created { get; set; }
        public string CIN { get; set; }
        public string DecouvertePlateForme { get; set; }
        public bool Oriente { get; set; }
        public bool Valide { get; set; } = false;
        public Statut Statut { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? DateAderation { get; set; }
        public ICollection<Diplome> Diplomes { get; set; }
        public ICollection<Document> Documents { get; set; }

    }
}


