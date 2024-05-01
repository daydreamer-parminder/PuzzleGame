using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataPrefs : AUserData
{
    public MatchEvent matchEvent;
    public int turns = 0;

    public override void LoadData()
    {
        // Load level and score from player prefs or other data source
        level = PlayerPrefs.GetInt("Level", 1);
        turns = PlayerPrefs.GetInt("Turns", 0);
        score = PlayerPrefs.GetInt("Score", 0);
        matchEvent(level, turns, score);
    }

    // method to increase turn
    public void IncreaseTurn()
    {
        turns += 1;
        SaveTurn(); // Save turn after increasing
    }

    public void SaveTurn()
    {
        // Save score to player prefs
        PlayerPrefs.SetInt("Turns", turns);
        PlayerPrefs.Save();
    }

    public override void SaveScore()
    {
        // Save score to player prefs
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    public override void SaveLevel()
    {
        // Save level to player prefs
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.Save();
    }
}
