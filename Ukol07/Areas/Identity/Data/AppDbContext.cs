using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ukol07.Models;

namespace Ukol07.Areas.Identity.Data;

public class AppDbContext : IdentityDbContext<User> {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) {
    }

    // Prázdný konstruktor
    public AppDbContext(){}

    public DbSet<BlogArticle> BlogArticles { get; set; }

    public DbSet<Comment> Comments { get; set; }
    
    public DbSet<User> Users { get; set; }

    // Zde si nastavíme další 2 vlastnosti uživatele -> tak aby byli zohledněny i v databázi
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        // Dodefinování vztahů 
        builder.Entity<BlogArticle>()
            .HasMany(a => a.Comments)
            .WithOne(c => c.RelevantArticle);

        builder.Entity<BlogArticle>()
            .HasOne(a => a.Author);
        
        builder.ApplyConfiguration(new UserEntityConfiguration());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseSqlite("Data Source = /Users/ondrejbercik/School - IT UPOL/Rider/Ukoly .NET/Ukol07/Ukol07.db");
    }
}

public class UserEntityConfiguration : IEntityTypeConfiguration<User> {
    
    public void Configure(EntityTypeBuilder<User> builder) {
        
        builder.Property(u => u.FirstName).HasMaxLength(255); 
        builder.Property(u => u.LastName).HasMaxLength(255);
        builder.Property(u => u.BlogNickname).HasMaxLength(255);
    }
}
