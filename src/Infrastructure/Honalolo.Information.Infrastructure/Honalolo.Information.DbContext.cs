using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using Honalolo.Information.Domain.Entities;
using Honalolo.Inforamtion.Domain.Entities.Attractions;
using Honalolo.Inforamtion.Domain.Entities.Locations;

namespace Honalolo.Information.Infrastructure
{
    public class TouristInfoDbContext : DbContext
    {
        public TouristInfoDbContext(DbContextOptions<TouristInfoDbContext> options) : base(options)
        {
        }

        // 1. Register all your Domain Entities as DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<AttractionType> AttractionTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Continent> Continents { get; set; }

        // Extension tables
        public DbSet<Event> Events { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Food> Foods { get; set; }

        // Child tables
        public DbSet<OpeningHour> OpeningHours { get; set; }
        public DbSet<AttractionLanguage> Languages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // === USERS MAPPING ===
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users"); // Table name in DB
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("user_id"); // [cite: 1]
                entity.Property(e => e.Role).HasColumnName("user_type").HasConversion<int>();
                entity.Property(e => e.UserName).HasColumnName("user_name").HasMaxLength(255);
                entity.Property(e => e.Email).HasColumnName("email").HasMaxLength(255);
                entity.Property(e => e.PasswordHash).HasColumnName("password").HasMaxLength(60);
            });

            // === ATTRACTIONS MAPPING ===
            modelBuilder.Entity<Attraction>(entity =>
            {
                entity.ToTable("Attractions");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("attraction_id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.LocationDetails).HasColumnName("location");
                entity.Property(e => e.ImagesJson).HasColumnName("images");
                entity.Property(e => e.Price).HasColumnName("price").HasColumnType("decimal(10,2)");

                // Relationships
                entity.HasOne(d => d.Author)
                      .WithMany(p => p.AuthoredAttractions)
                      .HasForeignKey(d => d.AuthorId)
                      .HasConstraintName("FK_Attraction_User"); // Optional: Name the constraint

                entity.HasOne(d => d.City)
                      .WithMany(p => p.Attractions)
                      .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Type)
                      .WithMany()
                      .HasForeignKey(d => d.TypeId);
            });

            // === CHILD TABLES (Opening Hours & Languages) ===
            modelBuilder.Entity<OpeningHour>(entity =>
            {
                entity.ToTable("Opening_hours");
                entity.HasKey(e => e.Id); 
                entity.Property(e => e.AttractionId).HasColumnName("attraction_id");
                entity.Property(e => e.Content).HasColumnName("opening_hours");

                entity.HasOne(d => d.Attraction)
                      .WithMany(p => p.OpeningHours)
                      .HasForeignKey(d => d.AttractionId);
            });

            modelBuilder.Entity<AttractionLanguage>(entity =>
            {
                entity.ToTable("Languages");
                entity.Property(e => e.AttractionId).HasColumnName("attraction_id"); 
                entity.Property(e => e.LanguageName).HasColumnName("languages");
            });

            // === LOCATION HIERARCHY ===
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("Cities");
                entity.Property(e => e.Id).HasColumnName("city_id"); 
                entity.Property(e => e.Name).HasColumnName("city_name");
                entity.HasOne(d => d.Region).WithMany().HasForeignKey(d => d.RegionId);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Regions");
                entity.Property(e => e.Id).HasColumnName("region_id");
                entity.Property(e => e.Name).HasColumnName("region_name");
                entity.HasOne(d => d.Country).WithMany().HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries");
                entity.Property(e => e.Id).HasColumnName("country_id");
                entity.Property(e => e.Name).HasColumnName("country_name");
                entity.HasOne(d => d.Continent).WithMany().HasForeignKey(d => d.ContinentId);
            });

            modelBuilder.Entity<Continent>(entity =>
            {
                entity.ToTable("Continents");
                entity.Property(e => e.Id).HasColumnName("continent_id");
                entity.Property(e => e.Name).HasColumnName("continent_name");
            });
        }
    }
}