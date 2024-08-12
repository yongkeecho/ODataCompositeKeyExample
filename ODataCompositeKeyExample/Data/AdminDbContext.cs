using ODataCompositeKeyExample.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ODataCompositeKeyExample.Data;

/// <summary>
/// Represents EF data context for equipment logbook admin.
/// </summary>
public class AdminDbContext : DbContext
{
    #region Properties

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Asset> Assets { get; set; }

    public virtual DbSet<AssetSme> AssetSmes { get; set; }
    
    #endregion

    #region Constructors

    public AdminDbContext(DbContextOptions<AdminDbContext> options)
      : base(options)
    { }

    #endregion

    #region Methods

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies().UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        var setConverter = new ValueConverter<ISet<int>, string>(s => string.Join(';', s), t => t.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x)).ToHashSet());

        modelBuilder.Entity<User>(b =>
        {
            b.ToTable("users");
            b.HasKey(u => u.Id);
            b.HasIndex(u => u.LastName);
            b.HasIndex(u => u.FirstName);
            b.HasIndex(u => u.IsAdmin);
            b.Property(u => u.Id).HasColumnName("id");
            b.Property(u => u.LastName).HasColumnName("lastName");
            b.Property(u => u.FirstName).HasColumnName("firstName");
            b.Property(u => u.DepartmentId).HasColumnName("departmentId");
            b.Property(u => u.ShowMyAssetsOnly).HasColumnName("showMyAssetsOnly");
            b.Property(u => u.MyAssets).HasConversion(setConverter).HasColumnName("myAssets");
            b.Property(u => u.IsAdmin).HasColumnName("isAdmin");
        });
        modelBuilder.Entity<User>().HasData(
            new User { Id = "amr@mycompany.com", LastName = "Unknown", FirstName = "Amr", DepartmentId = "AD" },
            new User { Id = "aren@mycompany.com", LastName = "Unknown", FirstName = "Aren", DepartmentId = "AD" }
        );

        modelBuilder.Entity<Department>(b =>
        {
            b.ToTable("departments");
            b.HasKey(d => d.Id);
            b.HasIndex(d => d.Name);
            b.Property(d => d.Id).HasColumnName("id");
            b.Property(d => d.Name).HasColumnName("name");
        });
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = "AB", Name = "AB" },
            new Department { Id = "AC", Name = "AC" },
            new Department { Id = "AD", Name = "AD" }
        );

        modelBuilder.Entity<Asset>(b =>
        {
            b.ToTable("assets");
            b.HasKey(a => a.Id);
            b.Property(a => a.Id).HasColumnName("id");
            b.Property(a => a.DepartmentId).HasColumnName("departmentId");
        });
        modelBuilder.Entity<Asset>().HasData(
            new Asset { Id = 100265, DepartmentId = "AB" },
            new Asset { Id = 100418, DepartmentId = "AC" },
            new Asset { Id = 100552, DepartmentId = "AD" },
            new Asset { Id = 100757, DepartmentId = "AD" },
            new Asset { Id = 100759, DepartmentId = "AC" },
            new Asset { Id = 101165, DepartmentId = "AD" },
            new Asset { Id = 101466, DepartmentId = "AD" },
            new Asset { Id = 101571, DepartmentId = "AD" },
            new Asset { Id = 102149, DepartmentId = "AD" }
        );
        modelBuilder.Entity<AssetSme>(b =>
        {
            b.ToTable("assetSmes");
            b.HasKey(s => new { s.AssetId, s.UserId });
            b.Property(s => s.AssetId).HasColumnName("assetId");
            b.Property(s => s.UserId).HasColumnName("userId");
            b.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId);
        });
        modelBuilder.Entity<AssetSme>().HasData(
            new AssetSme { AssetId = 100418, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 100265, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 101165, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 105437, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 108320, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 108397, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 108823, UserId = "aren@mycompany.com" },
            new AssetSme { AssetId = 108337, UserId = "aren@mycompany.com" },
            new AssetSme { AssetId = 109102, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 105714, UserId = "aren@mycompany.com" },
            new AssetSme { AssetId = 101466, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 105715, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 106047, UserId = "aren@mycompany.com" },
            new AssetSme { AssetId = 108849, UserId = "aren@mycompany.com" },
            new AssetSme { AssetId = 103040, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 109020, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 108756, UserId = "amr@mycompany.com" },
            new AssetSme { AssetId = 102149, UserId = "aren@mycompany.com" }
        );
    }

    #endregion
}