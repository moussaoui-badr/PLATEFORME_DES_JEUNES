namespace Domain.Models
{
    public class CreateClient
    {
        public CreateClient()
        {
            Exceptions = new List<string>();
        }

        public CandidatViewModel Candidat { get; set; }

        public List<DiplomeModel> DiplomeModel { get; set; }

        public List<DocumentModel> DocumentModel { get; set; }

        public List<SuiviFormationModel> SuiviFormationModel { get; set; }

        public List<string> Exceptions { get; set; }

    }
}
