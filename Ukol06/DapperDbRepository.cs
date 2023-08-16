using System.Data;
using Dapper;

namespace Ukol06; 

public class DapperDbRepository {
    private IDbConnection dbConnection;
    
    // SQL dotazy týkajících se hráčů
    private static string DELETE_PLAYER_SQL = "DELETE FROM Players WHERE PlayerId == @id";
    
    private static string INSERT_PLAYER_SQL =
        "INSERT INTO Players (Name, Age, SquadNumber, ClubId, Nationaly, Position, Height, Weight) " +
        "VALUES ( @name, @age, @squadNumber, @clubId, @nationaly, @position, @height, @weight)";

    private static string GET_PLAYER_BY_NAME_SQL =
        "SELECT * FROM Players AS p, Clubs AS c WHERE p.Name == @name AND p.ClubID == c.ClubID";
    
    private static string GET_PLAYER_BY_ID_SQL =
        "SELECT * FROM Players AS p, Clubs AS c WHERE p.PlayerId == @id AND p.ClubId == c.ClubId";
    
    private static string GET_ALL_PLAYERS_SQL =
        "SELECT * FROM Players AS p, Clubs AS c WHERE p.ClubId == c.ClubId";
    
    private static string UPDATE_PLAYER_SQL = 
        "UPDATE Players SET Name = @name, Age = @age, SquadNumber = @squadNumber, ClubId = @clubId, " +
        "Nationaly = @nationaly, Position = @position, Height = @height, Weight = @weight " +
        "WHERE PlayerId = @id";


    // ------------------------------------------------------------------------------------
    // SQL týkajících se klubů
    private static string DELETE_CLUB_SQL = "DELETE FROM Clubs WHERE ClubId = @id";
    
    private static string INSERT_CLUB_SQL =
        "INSERT INTO Clubs (Name, Coach, Shortcut, Country, City, FoundationYear, StadiumId) " +
        "VALUES (@name, @coach, @shortcut, @country, @city, @foundationYear, @stadiumId)";
    
    private static string GET_CLUB_BY_ID_SQL =
        "SELECT * FROM Clubs AS c, Stadiums AS s WHERE c.ClubId == @id AND c.StadiumId = s.StadiumId";
    
    private static string GET_CLUB_BY_NAME_SQL =
        "SELECT * FROM Clubs AS c, Stadiums AS s WHERE c.Name == @name AND c.StadiumId = s.StadiumId";

    private static string GET_ALL_CLUBS_SQL =
        "SELECT * FROM Clubs AS c, Stadiums AS s WHERE c.StadiumId == s.StadiumId";

    private static string UPDATE_CLUB_SQL =
        "UPDATE Clubs SET Name = @name, Coach = @coach, Shortcut = @shortcut, Country = @country, City = @city, " +
        "FoundationYear = @foundationYear, StadiumId = @stadiumId WHERE ClubId = @id";
    
    // ------------------------------------------------------------------------------------
    // SQL týkajících se stadionů
    private static string DELETE_STADIUM_SQL = "DELETE FROM Stadiums WHERE StadiumId = @id";
    
    private static string INSERT_STADIUM_SQL =
        "INSERT INTO Stadiums (Name, Capacity, City) " +
        "VALUES (@name, @capacity, @city)";

    private static string GET_STADIUM_BY_ID_SQL = "SELECT * FROM Stadiums WHERE StadiumId == @id";

    private static string GET_STADIUM_BY_NAME_SQL = "SELECT * FROM Stadiums WHERE Name == @name";

    private static string GET_ALL_STADIUMS_SQL = "SELECT * FROM Stadiums";

    private static string UPDATE_STADIUM_SQL = "UPDATE Stadiums SET Name = @name, Capacity = @capacity, " +
                                               "City = @city WHERE StadiumId = @id";
    
    // -------------------------------------------------------------------------
    // Ostatní pomocné dotazy
    private static string LAST_INSERT_ID_SQL = "SELECT last_insert_rowid();";


    // Konstruktor
    public DapperDbRepository(IDbConnection connection) {
        dbConnection = connection;
    }
    
    // --------------------------------------------------------------------------------------------------------
    // Hráči
    // Při mazání hráče nemažeme i klub jelikož v něm může ještě někdo hrát 
    public void DeletePlayer(Player player) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }
        
        dbConnection.Execute(DELETE_PLAYER_SQL, new {id = player.PlayerId});
    }

    // Když chci přidávat hráče musím přidat vše -> stadion i club
    public void InsertPlayer(Player player) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }
        
        InsertClub(player.Club);
        
        int clubId = dbConnection.ExecuteScalar<int>(LAST_INSERT_ID_SQL);

        // A konečne uložíme hráče
        dbConnection.Execute(
            INSERT_PLAYER_SQL,
            new {
                name = player.Name,
                age = player.Age,
                squadNumber = player.SquadNumber,
                clubId = clubId,
                nationaly = player.Nationaly,
                position = player.Position,
                height = player.Height,
                weight = player.Weight
            }
        );
    }

    // Pokud budu dostávat hráče musím ho dostat i s klubem a stadionem
    public Player GetPlayerByName(string name) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        Player player = dbConnection.Query<Player, Club, Player>(
            GET_PLAYER_BY_NAME_SQL,
            (player, club) => {
                player.Club = club;
                return player;
            },
            new { name = name },
            splitOn: "ClubId"
        ).Distinct().First();

        Stadium rightStadium = GetStadiumById(player.Club.StadiumId);
        player.Club.Stadium = rightStadium; 
        return player;
    }

    public Player GetPlayerById(int playerId) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }
        Player player = dbConnection.Query<Player, Club, Player>(
            GET_PLAYER_BY_ID_SQL,
            (player, club) => {
                player.Club = club;
                return player;
            },
            new { id = playerId },
            splitOn: "ClubId"
        ).Distinct().First();

        Stadium rightStadium = GetStadiumById(player.Club.StadiumId);
        player.Club.Stadium = rightStadium;
        return player;
    }

    public List<Player> GetAllPlayers() {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        List<Player> players = dbConnection.Query<Player, Club, Player>(
                GET_ALL_PLAYERS_SQL,
                (player, club) => {
                    player.Club = club;
                    return player;
                },
                splitOn: "ClubId")
            .Distinct().ToList();

        foreach (var player in players) {
            Stadium rightStadium = GetStadiumById(player.Club.StadiumId);
            player.Club.Stadium = rightStadium;
        }

        return players;
    }

    public void UpdatePlayer(Player player) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        dbConnection.Execute(
            UPDATE_PLAYER_SQL,
            new {
                name = player.Name,
                age = player.Age,
                squadNumber = player.SquadNumber,
                clubId = player.Club.ClubId,
                nationaly = player.Nationaly,
                position = player.Position,
                height = player.Height,
                weight = player.Weight,
                id = player.PlayerId
            }
        );
    }

    // ---------------------------------------------------------------------------------------------------
    // Stadiony
    public void DeleteStadium(Stadium stadium) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        dbConnection.Execute(DELETE_STADIUM_SQL, new {id = stadium.StadiumId});
    }

    public void InsertStadium(Stadium stadium) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        dbConnection.Execute(
            INSERT_STADIUM_SQL,
            new {
                name = stadium.Name,
                capacity = stadium.Capacity,
                city = stadium.City,
            }
        );
    }
    
    public Stadium GetStadiumById(int id) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }
        return dbConnection.Query<Stadium>(
            GET_STADIUM_BY_ID_SQL,
            new { id = id }
        ).Distinct().First();
    }
    
    public Stadium GetStadiumByName(string name) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }
        return dbConnection.Query<Stadium>(
            GET_STADIUM_BY_NAME_SQL,
            new { name = name }
        ).Distinct().First();
    }

    public List<Stadium> GetAllStadiums() {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        List<Stadium> stadiums = dbConnection.Query<Stadium>(GET_ALL_STADIUMS_SQL).Distinct().ToList();
        return stadiums;
    }

    public void UpdateStadium(Stadium stadium) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        dbConnection.Execute(
            UPDATE_STADIUM_SQL,
            new {
                name = stadium.Name,
                capacity = stadium.Capacity,
                city = stadium.City,
                id = stadium.StadiumId
            }
        );
    }

    // ---------------------------------------------------------------------------------------------------
    // Kluby
    public void DeleteClub(Club club) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }
        dbConnection.Execute(DELETE_CLUB_SQL, new { id = club.ClubId });
    }
    
    public Club GetClubById(int id) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        return dbConnection.Query<Club, Stadium, Club>(
            GET_CLUB_BY_ID_SQL,
            (club, stadium) => {
                club.Stadium = stadium;
                return club;
            },
            new { id = id },
            splitOn: "StadiumId"
        ).Distinct().First();
    }

    public Club GetClubByName(string name) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        return dbConnection.Query<Club, Stadium, Club>(
            GET_CLUB_BY_NAME_SQL,
            (club, stadium) => {
                club.Stadium = stadium;
                return club;
            },
            new { name = name },
            splitOn: "StadiumId"
        ).Distinct().First();
    }

    public List<Club> GetAllClubs() {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        List<Club> clubs = dbConnection.Query<Club, Stadium, Club>(
                GET_ALL_CLUBS_SQL,
                (club, stadium) => {
                    club.Stadium = stadium;
                    return club;
                },
                splitOn: "StadiumId")
            .Distinct().ToList();
        return clubs;
    }

    public void InsertClub(Club club) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }
        InsertStadium(club.Stadium);
        int lastId = dbConnection.ExecuteScalar<int>(LAST_INSERT_ID_SQL);

        dbConnection.Execute(
            INSERT_CLUB_SQL,
            new {
                name = club.Name,
                coach = club.Coach,
                shortcut = club.Shortcut,
                country = club.Country,
                city = club.City,
                foundationYear = club.FoundationYear,
                stadiumId = lastId
            }
        );
    }

    public void UpdateClub(Club club) {
        if (dbConnection.State == ConnectionState.Closed) {
            dbConnection.Open();
        }

        dbConnection.Execute(
            UPDATE_CLUB_SQL,
            new {
                name = club.Name,
                coach = club.Coach,
                shortcut = club.Shortcut,
                country = club.Country,
                city = club.City,
                foundationYear = club.FoundationYear,
                stadiumId = club.Stadium.StadiumId,
                id = club.ClubId
            }
        );
    }
}