namespace Ukol01;

public class FlagData {
    
    // Konstruktor
    public FlagData(string name, uint landmass, uint zone, uint areaInThousandsKm, uint populationInMillions, uint language, uint religion, uint bars, uint stripes, uint colorsNumber, bool redPresence, bool greenPresence, bool bluePresence, bool goldOrYellowPresence, bool whitePresence, bool blackPresence, bool orangeOrBrownPresence, string majorColor, uint circlesNumber, uint uprightCrossesNumber, uint diagonalCrossesNumber, uint sectionsNumber, uint starsOrSunsNumber, bool moonSymbolPresence, bool trialglesPresence, bool inanimateIconPresence, bool animateIconPresence, bool textPresence, string topLeftCornerColor, string bottomRightCornerColor) {
        Name = name;
        Landmass = landmass;
        Zone = zone;
        AreaInThousandsKm = areaInThousandsKm;
        PopulationInMillions = populationInMillions;
        Language = language;
        Religion = religion;
        Bars = bars;
        Stripes = stripes;
        ColorsNumber = colorsNumber;
        RedPresence = redPresence;
        GreenPresence = greenPresence;
        BluePresence = bluePresence;
        GoldOrYellowPresence = goldOrYellowPresence;
        WhitePresence = whitePresence;
        BlackPresence = blackPresence;
        OrangeOrBrownPresence = orangeOrBrownPresence;
        MajorColor = majorColor;
        CirclesNumber = circlesNumber;
        UprightCrossesNumber = uprightCrossesNumber;
        DiagonalCrossesNumber = diagonalCrossesNumber;
        SectionsNumber = sectionsNumber;
        StarsOrSunsNumber = starsOrSunsNumber;
        MoonSymbolPresence = moonSymbolPresence;
        TrialglesPresence = trialglesPresence;
        InanimateIconPresence = inanimateIconPresence;
        AnimateIconPresence = animateIconPresence;
        TextPresence = textPresence;
        TopLeftCornerColor = topLeftCornerColor;
        BottomRightCornerColor = bottomRightCornerColor;
    }

    public string Name { get; set; }
    public uint Landmass { get; set; }
    public uint Zone { get; set; }
    public uint AreaInThousandsKm { get; set; }
    public uint PopulationInMillions { get; set; }
    public uint Language { get; set; }
    public uint Religion { get; set; }
    public uint Bars { get; set; }
    public uint Stripes { get; set; }
    public uint ColorsNumber { get; set; }
    public Boolean RedPresence { get; set; }
    public Boolean GreenPresence { get; set; }
    public Boolean BluePresence { get; set; }
    public Boolean GoldOrYellowPresence { get; set; }
    public Boolean WhitePresence { get; set; }
    public Boolean BlackPresence { get; set; }
    public Boolean OrangeOrBrownPresence { get; set; }
    public string MajorColor { get; set; }
    public uint CirclesNumber { get; set; }
    public uint UprightCrossesNumber { get; set; }
    public uint DiagonalCrossesNumber { get; set; }
    public uint SectionsNumber { get; set; }
    public uint StarsOrSunsNumber { get; set; }
    public Boolean MoonSymbolPresence { get; set; }
    public Boolean TrialglesPresence { get; set; }
    public Boolean InanimateIconPresence { get; set; }
    public Boolean AnimateIconPresence { get; set; }
    public Boolean TextPresence { get; set; }
    public string TopLeftCornerColor { get; set; }
    public string BottomRightCornerColor { get; set; }
    
    // Přepsání metody toString    
    public override string ToString() {
        return $"{Name}, {ELandmass.GetValues(typeof(ELandmass)).GetValue(Landmass - 1)}";
    }

    // Výčtové třídy 
    public enum ELandmass : short {
        NORTH_AMERICA = 1,
        SOUTH_AMERICA = 2,
        EUROPE = 3,
        AFRICA = 4,
        ASIA = 5,
        OCEANIA = 6
    }

    public enum EZone : short {
        NE = 1,
        SE = 2,
        SW = 3,
        NW = 4,
    }

    public enum ELanguages : short {
        ENGLISH = 1,
        SPANISH = 2,
        FRENCH = 3,
        GERMAN = 4,
        SLAVIC = 5, 
        OTHER_INDO_EUROPEAN = 6,
        CHINESE = 7, 
        ARABIC = 8,
        JAPANESE = 9,
        TURKISH = 9,
        FINNISH = 9,
        MAGYAR = 9,
        OTHERS = 10
    }

    public enum EReligion {
        CATHOLIC = 0,
        OTHER_CHRISTIAN = 1,
        MUSLIM = 2,
        BUDDHIST = 3,
        HINDU = 4,
        ETHNIC = 5,
        MARXIST = 6,
        OTHERS = 7
    }
}