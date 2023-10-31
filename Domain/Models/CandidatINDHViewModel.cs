using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CandidatINDHViewModel
    {
        public int ClientID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        public IFormFile ImageUrl { get; set; }
        public string ImageUrlString { get; set; }
        public string Commentaire { get; set; }
        public string Nom { get; set; }
        public string CIN { get; set; }
        public string Prenom { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }
        public SexeC Sexe { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Designation { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? DateAderation { get; set; }
        public PlateformeGestionnaire PlateformeGestionnaire { get; set; }
        public DateTime Created { get; set; }
        public double MontantProjet { get; set; }
        public double MontantApportPersonnel { get; set; }
        public double MontantINDH { get; set; }
        public double TotalDevis { get; set; }
        public double Ecart { get; set; }
        public bool ApportPersonnelConfirme { get; set; }
    }
}
