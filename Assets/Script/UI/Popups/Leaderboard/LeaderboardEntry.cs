using System.Collections.Generic;

[System.Serializable]
public class LeaderboardEntry
{
    public string id;
    public string name;
    public int highScore;
    public int rank;  // Temporarily store rank, not from JSON
}

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> items;
}
