namespace Ukol06; 

public class Club {
    public int ClubId { get; set; }
    public string Name { get; set; }
    public string Coach { get; set; }
    public string Shortcut { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public int FoundationYear { get; set; }
    public virtual Stadium Stadium { get; set; }
    
    public int StadiumId { get; set; }
}