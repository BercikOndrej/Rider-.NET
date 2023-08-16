using System.Data;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Ukol06;

class Program {
    private static List<Club> Clubs = new List<Club>();
    private static List<Player> Players = new List<Player>();
    private static List<Stadium> Stadiums = new List<Stadium>();

    private static readonly string ConnectionString =
        "Data source =/Users/ondrejbercik/School - IT UPOL/Rider/Ukoly .NET/Ukol06/FotballDb.sqlite";

    static void Main(string[] args) {
        // Vytvoření objektů, se kterými budu pracovat
        CreateData();
        
        // Práce s databází pomocí EF Core
        try {
            using (FotballContext ctx = new FotballContext()) {
                
                Stopwatch efFillDataSw = new Stopwatch();
                efFillDataSw.Start();
                FillDatabaseWithDataByEfc(ctx);
                efFillDataSw.Stop();
                
                Stopwatch efEditDataSw = new Stopwatch();
                efEditDataSw.Start();
                EditDataByEfc(ctx);
                efEditDataSw.Stop();

                Stopwatch efDeleteAllDataSw = new Stopwatch();
                efDeleteAllDataSw.Start();
                DeleteAllDataInDatabaseByEfc(ctx);
                efDeleteAllDataSw.Stop();
                
                
                // Výpis časů
                Console.WriteLine("ORM pomocí Entity Framework Core:");
                Console.WriteLine($"Čas naplnění databáze daty: {efFillDataSw.ElapsedMilliseconds} milisekund");
                Console.WriteLine($"Čas náhodné editace dat: {efEditDataSw.ElapsedMilliseconds} milisekund");
                Console.WriteLine($"Čas smazání všech dat z databáze: {efDeleteAllDataSw.ElapsedMilliseconds} milisekund");
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
        
        // Vynulování a znovuvytvoření dat
        Players = new List<Player>();
        Clubs = new List<Club>();
        Stadiums = new List<Stadium>();
        CreateData();
        Console.Write("\n\n");
        
        // Práce s databází pomocí frameworku Dapper
        try {
            using (IDbConnection db = new SqliteConnection(ConnectionString)) {
                DapperDbRepository repository = new DapperDbRepository(db);

                Stopwatch dapperFillDataSw = new Stopwatch();
                dapperFillDataSw.Start();
                FillDatabaseWithDataByDapper(repository);
                dapperFillDataSw.Stop();

                Stopwatch dapperEditDataSw = new Stopwatch();
                dapperEditDataSw.Start();
                EditDataByDapper(repository);
                dapperEditDataSw.Stop();

                Stopwatch dapperDeleteDataSw = new Stopwatch();
                dapperDeleteDataSw.Start();
                DeleteAllDataInDatabaseByDapper(repository);
                dapperDeleteDataSw.Stop();
                
                // Výpis časů
                Console.WriteLine("ORM pomocí frameworku Dapper:");
                Console.WriteLine($"Čas naplnění databáze daty: {dapperFillDataSw.ElapsedMilliseconds} milisekund");
                Console.WriteLine($"Čas náhodné editace dat: {dapperEditDataSw.ElapsedMilliseconds} milisekund");
                Console.WriteLine($"Čas smazání všech dat z databáze: {dapperDeleteDataSw.ElapsedMilliseconds} milisekund");
            }
        }
        catch (Exception e) {
            Console.WriteLine(e);
            throw;
        }
    }

    // Statické metody pro EFC
    // Editovat budu Hráče Saku -> změním číslo na 77, a jeho club na barcelonu
    static void EditDataByEfc(FotballContext ctx) {
        Club club = ctx.Clubs.FirstOrDefault(c => c.Name == "Barcelona FC");
                
        Player player = ctx.Players.FirstOrDefault(p => p.Name == "Bukayo Saka");
        if (player != null && club != null) {
            player.SquadNumber = 77;
            player.Club = club;
            ctx.SaveChanges();
        }
    }
    static void FillDatabaseWithDataByEfc(FotballContext ctx) {
        // Díky EF Core stačí přidat pouze hráče -> na ostatní je reference
        foreach (var player in Players) {
            ctx.Players.Add(player);
        }
        ctx.SaveChanges();
    }

    static void DeleteAllDataInDatabaseByEfc(FotballContext ctx) {
        foreach (var club in ctx.Clubs) {
            ctx.Clubs.Remove(club);
        }

        foreach (var player in ctx.Players) {
            ctx.Players.Remove(player);
        }

        foreach (var stadium in ctx.Stadiums) {
            ctx.Stadiums.Remove(stadium);
        }

        ctx.SaveChanges();
    }

    // Statické metody pro Dapper
    static void FillDatabaseWithDataByDapper(DapperDbRepository repo) {
        // Díky struktuře dat také stačí přidat pouze hráče
        foreach (Player player in Players) {
            repo.InsertPlayer(player);
        }
    }

    static void DeleteAllDataInDatabaseByDapper(DapperDbRepository repo) {
        var players = repo.GetAllPlayers();
        foreach (var player in players) {
            repo.DeletePlayer(player);
        }

        var clubs = repo.GetAllClubs();
        foreach (var club in clubs) {
            repo.DeleteClub(club);
        }

        var stadiums = repo.GetAllStadiums();
        foreach (var stadium in stadiums) {
            repo.DeleteStadium(stadium);
        }
    }

    static void EditDataByDapper(DapperDbRepository repo) {
        Club barcelona = repo.GetClubByName("Barcelona FC");
        Player saka = repo.GetPlayerByName("Bukayo Saka");
        saka.SquadNumber = 77;
        saka.Club = barcelona;
        repo.UpdatePlayer(saka);
    }
    
    
    // Vytvoření dat manuálně
    static void CreateData() {
        // Toto bude delší metoda, budu zde vytvářet objekty ručně -> vždy vytvořím club, stadion a hráče
        
        Stadium s1 = new Stadium() {
            Name = "Old Traford",
            City = "Manchester",
            Capacity = 74140
        };
        
        Club c1 = new Club() {
            Name = "Manchester United FC",
            Shortcut = "MUFC",
            Coach = "Erik ten Hag",
            City = "Manchester",
            FoundationYear = 1878,
            Country = "Anglie",
            Stadium = s1
        };
        
        Player p1 = new Player() {
            Name = "Cristiano Ronaldo",
            Age = 37,
            SquadNumber = 7,
            Height = 187,
            Weight = 85,
            Position = "Útočník",
            Nationaly = "Portugalsko",
            Club = c1
        };
        Clubs.Add(c1);
        Players.Add(p1);
        Stadiums.Add(s1);
        
        Stadium s2 = new Stadium() {
            Name = "Etihad Stadium",
            City = "Manchester",
            Capacity = 55097,
        };

        Club c2 = new Club() {
            Name = "Manchester City FC",
            Shortcut = "CITY",
            Coach = "Josep Guardiola",
            City = "Manchester",
            FoundationYear = 1880,
            Country = "Anglie",
            Stadium = s2
        };

        Player p2 = new Player() {
            Name = "Phil Foden",
            Age = 22,
            SquadNumber = 47,
            Height = 171,
            Weight = 70,
            Position = "Záložník",
            Nationaly = "Anglie",
            Club = c2
        };
        
        Clubs.Add(c2);
        Players.Add(p2);
        Stadiums.Add(s2);
        
        Stadium s3 = new Stadium() {
            Name = "Emirates Stadium",
            City = "London",
            Capacity = 60260
        };
        
        Club c3 = new Club() {
            Name = "Arsenal FC",
            Shortcut = "ARS",
            Coach = "Mikel Arteta",
            City = "London",
            FoundationYear = 1886,
            Country = "Anglie",
            Stadium = s3
        };

        Player p3 = new Player() {
            Name = "Bukayo Saka",
            Age = 21,
            SquadNumber = 7,
            Height = 178,
            Weight = 65,
            Position = "Záložník",
            Nationaly = "Anglie",
            Club = c3
        };
        
        Clubs.Add(c3);
        Players.Add(p3);
        Stadiums.Add(s3);
        
        Stadium s4 = new Stadium() {
            Name = "Santiago Bernabéu",
            City = "Madrid",
            Capacity = 81044
        };
        
        Club c4 = new Club() {
            Name = "Real Madrid FC",
            Shortcut = "RMA",
            Coach = "Carlo Ancelotti",
            City = "Madrid",
            FoundationYear = 1902,
            Country = "Španělsko",
            Stadium = s4
        };

        Player p4 = new Player() {
            Name = "Karim Benzema",
            Age = 34,
            SquadNumber = 9,
            Height = 185,
            Weight = 81,
            Position = "Útočník",
            Nationaly = "Francie",
            Club = c4
        };
        
        Clubs.Add(c4);
        Players.Add(p4);
        Stadiums.Add(s4);
        
        Stadium s5 = new Stadium() {
            Name = "Camp Nou",
            City = "Barcelona",
            Capacity = 99354
        };
        
        Club c5 = new Club() {
            Name = "Barcelona FC",
            Shortcut = "BAR",
            Coach = "Xavi",
            City = "Barcelona",
            FoundationYear = 1899,
            Country = "Španělsko",
            Stadium = s5
        };
        
        Player p5 = new Player() {
            Name = "Gavi",
            Age = 18,
            SquadNumber = 30,
            Height = 173,
            Weight = 68,
            Position = "Záložník",
            Nationaly = "Španělsko",
            Club = c5
        };
        
        Clubs.Add(c5);
        Players.Add(p5);
        Stadiums.Add(s5);
        
        Stadium s6 = new Stadium() {
            Name = "St. James' Park",
            City = "NewCastle",
            Capacity = 52405,
        };
        
        Club c6 = new Club() {
            Name = "Newcastle United FC",
            Shortcut = "NUFC",
            Coach = "Eddie Howe",
            City = "Newcastle",
            FoundationYear = 1892,
            Country = "Anglie",
            Stadium = s6
        };
        
        Player p6 = new Player() {
            Name = "Alexander Isak",
            Age = 23,
            SquadNumber = 14,
            Height = 192,
            Weight = 77,
            Position = "Útočník",
            Nationaly = "Švédsko",
            Club = c6
        };
        
        Clubs.Add(c6);
        Players.Add(p6);
        Stadiums.Add(s6);
        
        Stadium s7 = new Stadium() {
            Name = "Stamford Bridge",
            City = "London",
            Capacity = 41837
        };
        
        Club c7 = new Club() {
            Name = "Chelsea FC",
            Shortcut = "CHE",
            Coach = "Graham Potter",
            City = "London",
            FoundationYear = 1905,
            Country = "Anglie",
            Stadium = s7
        };

        Player p7 = new Player() {
            Name = "Mason Mount",
            Age = 23,
            SquadNumber = 19,
            Height = 180,
            Weight = 74,
            Position = "Záložník",
            Nationaly = "Anglie",
            Club = c7
        };
        
        Clubs.Add(c7);
        Players.Add(p7);
        Stadiums.Add(s7);
        
        Stadium s8 = new Stadium() {
            Name = "Anfield",
            City = "Liverpool",
            Capacity = 53394
        };
        
        Club c8 = new Club() {
            Name = "Liverpool FC",
            Shortcut = "LIV",
            Coach = "Jürgen Klopp",
            City = "Liverpool",
            FoundationYear = 1892,
            Country = "Anglie",
            Stadium = s8
        };

        Player p8 = new Player() {
            Name = "Mohamed Salah",
            Age = 30,
            SquadNumber = 11,
            Height = 175,
            Weight = 73,
            Position = "Záložník",
            Nationaly = "Egypt",
            Club = c8
        };
        
        Clubs.Add(c8);
        Players.Add(p8);
        Stadiums.Add(s8);
        
        Stadium s9 = new Stadium() {
            Name = "Parc des Princes",
            City = "Paris",
            Capacity = 47929
        };
        
        Club c9 = new Club() {
            Name = "Paris Saint-Germain",
            Shortcut = "PSG",
            Coach = "Christophe Galtier",
            City = "Paris",
            FoundationYear = 1970,
            Country = "Francie",
            Stadium = s9
        };

        Player p9 = new Player() {
            Name = "Kylian Mbappé",
            Age = 23,
            SquadNumber = 7,
            Height = 178,
            Weight = 73,
            Position = "Záložník",
            Nationaly = "Francie",
            Club = c9
        };
        
        Clubs.Add(c9);
        Players.Add(p9);
        Stadiums.Add(s9);
        
        Stadium s10 = new Stadium() {
            Name = "Juventus Stadium",
            City = "Turin",
            Capacity = 41507,
        };
        
        Club c10 = new Club() {
            Name = "Juventus FC",
            Shortcut = "JUV",
            Coach = "Massimiliano Allegri",
            City = "Turín",
            FoundationYear = 1897,
            Country = "Italie",
            Stadium = s10
        };

        Player p10 = new Player() {
            Name = "Paul Pogba",
            Age = 29,
            SquadNumber = 10,
            Height = 191,
            Weight = 84,
            Position = "Záložník",
            Nationaly = "Franice",
            Club = c10
        };
        
        Clubs.Add(c10);
        Players.Add(p10);
        Stadiums.Add(s10);
    }
}