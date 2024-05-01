using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataPrefs : AUserData
{
    public override void LoadData()
    {
        // Load level and score from player prefs or other data source
        level = PlayerPrefs.GetInt("Level", 1);
        score = PlayerPrefs.GetInt("Score", 0);
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
