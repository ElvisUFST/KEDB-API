using KEDB.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KEDB.Data
{
    public class KEDBContext : DbContext
    {

        public KEDBContext(DbContextOptions<KEDBContext> opt) : base(opt)
        {

        }

        public DbSet<Kontrolrapport> Kontrolrapporter { get; set; }
        public DbSet<AndreUregelmaessigheder> AndreUregelmaessigheder { get; set; }
        public DbSet<Rubrik> Rubrikker { get; set; }
        public DbSet<RubrikType> RubrikTyper { get; set; }
        public DbSet<Profil> Profiler { get; set; }
        public DbSet<Fejltekst> Fejltekster { get; set; }
        public DbSet<RubrikValgtFejl> RubrikValgteFejl { get; set; }
        public DbSet<RubrikMuligFejl> RubrikMuligeFejl { get; set; }
        public DbSet<Toldrapport> Toldrapporter { get; set; }
        public DbSet<ToldrapportTransportmiddel> ToldrapportTransportmiddeler { get; set; }
        public DbSet<ToldrapportOpdagendeAktoer> ToldrapportOpdagendeAktoer { get; set; }
        public DbSet<ToldrapportFejlKategori> ToldrapportFejlKategorier { get; set; }
        public DbSet<ToldrapportKommunikation> ToldrapportKommunikationer { get; set; }
        public DbSet<ToldrapportOvertraedelsesAktoer> ToldrapportOvertraedelsesAktoer { get; set; }


        /*
        protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.Entity<RubrikMuligFejl>()
            .HasKey(c => new { c.RubrikTypeID, c.ProfilId, c.FejltekstId });    
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //laver compound key 
            modelBuilder.Entity<RubrikMuligFejl>().HasKey(rmf => new { rmf.RubrikTypeId, rmf.ProfilId, rmf.FejltekstId });
            modelBuilder.Entity<RubrikValgtFejl>().HasKey(rf => new { rf.RubrikId, rf.FejltekstId });


            //opdatering af disse members skal ignoreres. Det skal ikke være muligt at ændre dem.
            modelBuilder.Entity<Kontrolrapport>(builder =>
            {
                builder.Property(e => e.Branchekode).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                builder.Property(e => e.KlarererCVR).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                builder.Property(e => e.Referencenummer).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                builder.Property(e => e.Varepostnummer).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                builder.Property(e => e.Toldsted).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                builder.Property(e => e.VaremodtagerCVR).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                builder.Property(e => e.VaremodtagerNavn).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                builder.Property(e => e.AntagetDato).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            });


            //opdatering af disse members skal ignoreres. Det skal ikke være muligt at ændre dem.
            modelBuilder.Entity<Rubrik>(builder =>
            {
                //builder.Property(e => e.OriginalVaerdi).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
                //builder.Property(e => e.RubrikType).Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
            });

            //ModelBuilderExtensions.cs
            modelBuilder.Seed();
        }
    }
}