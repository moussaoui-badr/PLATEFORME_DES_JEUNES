using Domain.Entities;
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
        public DbSet<Famille> Familles { get; set; }
        public DbSet<Secteur> Secteurs { get; set; }
        public DbSet<TypeRelationParente> TypesRelationParente { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Personne>()
                .HasOne(p => p.Famille) // Une personne appartient à une seule famille
                .WithMany(f => f.Membres) // Une famille peut avoir plusieurs membres
                .HasForeignKey(p => p.FamilleId); // Clé étrangère dans Personne

            modelBuilder.Entity<Famille>()
                .HasMany(f => f.Membres) // Une famille peut avoir plusieurs membres
                .WithOne(p => p.Famille) // Un membre appartient à une seule famille
                .HasForeignKey(p => p.FamilleId); // Clé étrangère dans Personne

            base.OnModelCreating(modelBuilder);
        }
    }
}
