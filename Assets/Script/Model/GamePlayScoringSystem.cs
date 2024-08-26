using System;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayScoringSystem 
{
    private readonly List<Rank> currentSequence = new List<Rank>();
    private int currentScore = 0;

    // Event to notify subscribers about score changes
    public event Action<int> OnScoreChanged;

    // Function to add a card to the sequence and update the score
    public void AddCardToSequence(Rank cardRank)
    {
        currentSequence.Add(cardRank);
        UpdateScore();  // Calculate and update the score based on the new sequence
    }

    // Function to reset the sequence without resetting the total score
    public void ResetSequence()
    {
        currentSequence.Clear();  // Only clear the sequence, not the score
    }

    // Updates the score based on the current sequence length and notifies observers
    private void UpdateScore(bool isReset = false)
    {
        if (!isReset)
        {
            currentScore += currentSequence.Count;
        }
        else
        {
            // Optionally handle what happens on reset differently if needed
        }

        // Notify all subscribers about the score change
        OnScoreChanged?.Invoke(currentScore);
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
}
