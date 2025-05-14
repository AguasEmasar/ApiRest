using LOGIN.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<UserEntity, IdentityRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema("Security");

        // Configuración de las tablas de Identity
        builder.Entity<UserEntity>().ToTable("users");
        builder.Entity<IdentityRole>().ToTable("roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("users_roles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("users_claims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("users_logins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("roles_claims");
        builder.Entity<IdentityUserToken<string>>().ToTable("users_tokens");

        // Configuración adicional para UserEntity
        builder.Entity<UserEntity>()
               .Property(u => u.CreatedDate)
               .IsRequired()
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Entity<UserEntity>()
               .Property(u => u.FirstName)
               .IsRequired()
               .HasMaxLength(50)
               .HasColumnName("first_name");

        builder.Entity<UserEntity>()
               .Property(u => u.LastName)
               .IsRequired()
               .HasMaxLength(50)
               .HasColumnName("last_name");

        builder.Entity<UserEntity>()
               .Property(u => u.UserName)
               .IsRequired();

        builder.Entity<UserEntity>()
               .Property(u => u.Email)
               .IsRequired();

        builder.Entity<ReportEntity>()
            .HasOne(r => r.State)
            .WithMany()
            .HasForeignKey(r => r.StateId)
            .IsRequired();

        // Configuración de la relación de muchos a muchos para RegistrationWater
        builder.Entity<RegistrationWaterEntity>()
            .HasMany(rw => rw.RegistrationWaterNeighborhoodsColonies)
            .WithOne(rwnc => rwnc.RegistrationWater)
            .HasForeignKey(rwnc => rwnc.RegistrationWaterId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configuración de la tabla intermedia RegistrationWaterNeighborhoodsColonies
        builder.Entity<RegistrationWaterNeighborhoodsColoniesEntity>()
            .HasKey(rwnc => new { rwnc.RegistrationWaterId, rwnc.NeighborhoodColoniesId });

        builder.Entity<RegistrationWaterNeighborhoodsColoniesEntity>()
            .HasOne(rwnc => rwnc.RegistrationWater)
            .WithMany(rw => rw.RegistrationWaterNeighborhoodsColonies)
            .HasForeignKey(rwnc => rwnc.RegistrationWaterId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RegistrationWaterNeighborhoodsColoniesEntity>()
            .HasOne(rwnc => rwnc.NeighborhoodsColonies)
            .WithMany()
            .HasForeignKey(rwnc => rwnc.NeighborhoodColoniesId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    // DbSets para las entidades
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CommunicateEntity> Communicates { get; set; }
    public DbSet<StateEntity> States { get; set; }
    public DbSet<ReportEntity> Reports { get; set; }
    public DbSet<NeighborhoodsColoniesEntity> NeighborhoodsColonies { get; set; }
    public DbSet<BlocksEntity> Blocks { get; set; }
    public DbSet<DistrictsPointsEntity> Districts { get; set; }
    public DbSet<LinesEntity> Lines { get; set; }
    public DbSet<RegistrationWaterEntity> RegistrationWater { get; set; }
    public DbSet<RegistrationWaterNeighborhoodsColoniesEntity> RegistrationWaterNeighborhoodsColonies { get; set; }

}
