using System.Collections.Generic;
using UnityEngine;

public class LeaderboardContent : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform contentPanel;

    public void Initialize()
    {
        LeaderboardData loadedData = FetchLeaderboardData();
        ProcessData(loadedData);
        RenderEntries(loadedData.items);
    }

    private LeaderboardData FetchLeaderboardData()
    {
        Debug.Log("Loading leaderboard data");
        TextAsset json = Resources.Load<TextAsset>("leaderboard");
        if (json == null)
        {
            Debug.LogError("Failed to load leaderboard JSON data!");
            return null;
        }

        LeaderboardData loadedData = JsonUtility.FromJson<LeaderboardData>(json.text);
        if (loadedData == null || loadedData.items == null)
        {
            Debug.LogError("Failed to parse leaderboard data!");
        }

        // Add the player's high score to the list
        loadedData.items.Add(new LeaderboardEntry { name = "You", highScore = PlayerPrefs.GetInt("HighScore", 0) });
       
        return loadedData;
    }

    private void ProcessData(LeaderboardData data)
    {
        SortEntries(data.items);
        AssignRanks(data.items);
    }

    private void SortEntries(List<LeaderboardEntry> entries)
    {
        entries.Sort((a, b) => b.highScore.CompareTo(a.highScore));
    }

    private void AssignRanks(List<LeaderboardEntry> entries)
    {
        for (int i = 0; i < entries.Count; i++)
        {
            entries[i].rank = i + 1;
        }
    }

    private void RenderEntries(List<LeaderboardEntry> entries)
    {
        foreach (var entry in entries)
        {
            GameObject newEntry = Instantiate(entryPrefab, contentPanel);
            newEntry.GetComponent<LeaderboardContentItem>().SetData(entry.rank, entry.name, entry.highScore);
        }
    }
}

