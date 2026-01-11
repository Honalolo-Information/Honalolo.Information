using Honalolo.Information.Domain.Entities.Attractions;
using Honalolo.Information.Domain.Entities.Locations;
using Microsoft.EntityFrameworkCore;

namespace Honalolo.Information.Infrastructure.Persistance
{
    public class TouristInfoDbContext : DbContext
    {
        public TouristInfoDbContext(DbContextOptions<TouristInfoDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<AttractionType> AttractionTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Continent> Continents { get; set; }

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
                entity.Property(e => e.Id).HasColumnName("user_id");
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

                entity.HasOne(d => d.Region)
                      .WithMany(p => p.City)
                      .HasForeignKey(d => d.RegionId);
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.ToTable("Regions");
                entity.Property(e => e.Id).HasColumnName("region_id");
                entity.Property(e => e.Name).HasColumnName("region_name");

                entity.HasOne(d => d.Country)
                      .WithMany(p => p.Regions)
                      .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries");
                entity.Property(e => e.Id).HasColumnName("country_id");
                entity.Property(e => e.Name).HasColumnName("country_name");

                entity.HasOne(d => d.Continent)
                      .WithMany(p => p.Countries)
                      .HasForeignKey(d => d.ContinentId);
            });

            modelBuilder.Entity<Continent>(entity =>
            {
                entity.ToTable("Continents");
                entity.Property(e => e.Id).HasColumnName("continent_id");
                entity.Property(e => e.Name).HasColumnName("continent_name");
            });

            // === EXTENSION TABLES MAPPING ===
            // 1. Events
            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("Events");
                entity.Property(e => e.AttractionId).HasColumnName("attraction_id");
                entity.Property(e => e.EventType).HasColumnName("event_type");
                entity.Property(e => e.StartDate).HasColumnName("start_date");
                entity.Property(e => e.EndDate).HasColumnName("end_date");
            });

            // 2. Food
            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("Food");
                entity.HasKey(e => e.Id); // Uses standard PK
                entity.Property(e => e.Id).HasColumnName("food_id"); // Give it its own PK column
                entity.Property(e => e.AttractionId).HasColumnName("attraction_id"); // Map the FK
                entity.Property(e => e.CuisineType).HasColumnName("food_type");
            });

            // 3. Hotels
            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotels");
                entity.HasKey(e => e.Id); // Uses standard PK
                entity.Property(e => e.Id).HasColumnName("hotel_id"); // Give it its own PK column
                entity.Property(e => e.AttractionId).HasColumnName("attraction_id");
                entity.Property(e => e.AmenitiesJson).HasColumnName("amenities");
            });

            // 4. Trails
            modelBuilder.Entity<Trail>(entity =>
            {
                entity.ToTable("Trails");
                entity.Property(e => e.AttractionId).HasColumnName("attraction_id");
                entity.Property(e => e.DistanceMeters).HasColumnName("distance_meters");
                entity.Property(e => e.AltitudeMeters).HasColumnName("attlitude_meters"); // Note: Schema diagram had a typo "attlitude". Check your DB!
                entity.Property(e => e.StartingPoint).HasColumnName("starting_point");
                entity.Property(e => e.EndpointPoint).HasColumnName("endpoint_point");
                // Map the Enum FK
                entity.Property(e => e.DifficultyLevelId).HasColumnName("attraction_diffculty_level");
            });

            // 5. Attraction Types
            modelBuilder.Entity<AttractionType>(entity =>
            {
                entity.ToTable("Attraction_types"); // Explicit table name
                entity.Property(e => e.Id).HasColumnName("attraction_type_id");
                entity.Property(e => e.TypeName).HasColumnName("type_name");
            });

        }
    }
}