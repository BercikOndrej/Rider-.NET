namespace Ukol06; 

public class Player {
    public int PlayerId { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public int SquadNumber { get; set; }
    public virtual Club Club { get; set; }
    public string Nationaly { get; set; }
    public string Position { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
}