namespace ScheduleApp;

public class Match()
{
    public int Id { get; set; }
    public string HomeTeam { get; set; }
    public string AwayTeam { get; set; }
    public DateTime Date { get; set; }
    public string Sport { get; set; }
    public string League { get; set; }
    public string Status { get; set; } = "Scheduled";
    public string Score { get; set; } = "";
    public string Venue { get; set; } = "";
}

public class Team()
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Sport { get; set; }
    public string League { get; set; }
    public bool IsFavorite { get; set; }
    public string Country { get; set; } = "";
    public string LogoPath { get; set; } = "";
}

public enum MatchStatus
{
    Scheduled,
    Live,
    Finished,
    Canceled,
    Postponed
}

public enum SportType
{
    Football,
    Lol,
    CS2,
    Valorant,
    Dota2
}