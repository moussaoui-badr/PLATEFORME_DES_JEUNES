using Domain.Entities;
using Domain.Entities.V2;
using KhalfiElection.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Repository.Data
{
#nullable disable
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Diplome> Diplomes { get; set; }
        public DbSet<Formation> Formations { get; set; }
        public DbSet<InscriptionFormation> InscriptionFormation { get; set; }
        public DbSet<ClientPublic> ClientsPublic { get; set; }
        public DbSet<EmailEnvoi> EmailEnvois { get; set; }
        public DbSet<ClientFinance> ClientFinances { get; set; }
        public DbSet<INDH> INDHS { get; set; }
        public DbSet<DocumentINDH> DocumentINDHS { get; set; }
        public DbSet<Chapitre> Chapitres { get; set; }
        public DbSet<Financement> Financements { get; set; }
        public DbSet<Fonctionnement> Fonctionnements { get; set; }
        public DbSet<BudgetFinancement> BudgetFinancement { get; set; }
        public DbSet<BudgetFonctionnement> BudgetFonctionnement { get; set; }


        public DbSet<Personne> Personnes { get; set; }
        public DbSet<Secteur> Secteurs { get; set; }
        public DbSet<TypeRelationParente> TypesRelationParente { get; set; }


        public DbSet<PersonnePivot> PersonnePivot { get; set; }
        public DbSet<PersonneResponsable> PersonneResponsable { get; set; }
        public DbSet<PersonneMembre> PersonneMembre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Personne>()
            .HasOne(p => p.ResponsableFamille) // Définit la relation avec la propriété ResponsableFamille
            .WithMany() // Indique qu'il peut y avoir plusieurs Personne liées à un ResponsableFamille
            .HasForeignKey(p => p.ResponsableFamilleId); // Clé étrangère

            base.OnModelCreating(modelBuilder);
        }
    }
}
