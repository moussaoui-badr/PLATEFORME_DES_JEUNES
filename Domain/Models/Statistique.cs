namespace Domain.Models
{
    public class Statistique
    {
        public int candidatAcceuilli { get; set; }
        public int nombreFemmes { get; set; }
        public int nombreHommes { get; set; }
        public int nombrePorteurDeProjets { get; set; }
        public int nombreChercheurEmploit { get; set; }
        public int nombreFormation { get; set; }
        public int nombreCandidatAcceuilliEtOriente { get; set; }
        public int nombreCandidatRefusé { get; set; }
        public int nombreCandidatNonOrienté { get; set; }
        public List<FormationModel> formations { get; set; }
    }
}
