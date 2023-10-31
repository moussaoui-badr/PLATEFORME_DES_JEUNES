using Domain.Entities;

namespace Domain.Models
{
    public class CreateClientINDH
    {
        public CandidatINDHViewModel Candidat { get; set; }

        public List<DocumentINDH> DocumentModel { get; set; }
        public List<INDH> INDHs { get; set; }

    }
}
