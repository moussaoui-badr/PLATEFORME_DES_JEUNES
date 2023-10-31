using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class CandidatPublicViewModel
    {
        public int ClientPublicID { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        [Required(AllowEmptyStrings = true)]
        public IFormFile ImageUrl { get; set; }
        public string ImageUrlString { get; set; }
        public string Nom { get; set; }
        public string CIN { get; set; }
        public string Prenom { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime DateNaissance { get; set; }
        public SituationFamilial SituationFamilial { get; set; }
        public Sexe Sexe { get; set; }
        public string Adresse { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string DecouvertePlateForme { get; set; }
        public Statut Statut { get; set; }
        public string Designation { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime? DateAderation { get; set; }
        public DateTime Created { get; set; }
        public bool Oriente { get; set; }
    }
}
