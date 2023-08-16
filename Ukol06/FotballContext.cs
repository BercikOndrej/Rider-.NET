using Microsoft.EntityFrameworkCore;

namespace Ukol06; 

public class FotballContext : DbContext{
    
    
    public DbSet<Club> Clubs { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Stadium> Stadiums { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
        options.UseLazyLoadingProxies()
            .UseSqlite("Data source =/Users/ondrejbercik/School - IT UPOL/Rider/Ukoly .NET/Ukol06/FotballDb.sqlite");
    }
}