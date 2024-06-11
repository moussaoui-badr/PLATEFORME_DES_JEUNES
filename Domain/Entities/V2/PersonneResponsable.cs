using KhalfiElection.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.V2
{
    public class PersonneResponsable
    {
        public int PersonneResponsableId { get; set; }
        public string? Nom { get; set; }
        public string? Prenom { get; set; }
        public string? CIN { get; set; }
        public DateTime? DateNaissance { get; set; }
        public string? Adresse { get; set; }
        public string? GSM { get; set; }
        public DateTime DateCreation { get; set; } = DateTime.Now;

        public int? SecteurId { get; set; }
        public Secteur? Secteur { get; set; }

        [Required(ErrorMessage = "Le pivot est obligatoire.")]
        public int? PivotId { get; set; }
        public PersonnePivot? Pivot { get; set; }

        public List<PersonneMembre>? Membres { get; set; }
    }
}
