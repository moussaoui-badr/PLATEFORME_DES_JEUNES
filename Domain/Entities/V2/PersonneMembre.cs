using KhalfiElection.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.V2
{
    public class PersonneMembre
    {
        public int PersonneMembreId { get; set; }
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

        public int? ResponsableId { get; set; }
        public PersonneResponsable? Responsable { get; set; }

        public int? PivotId { get; set; }
    }
}
