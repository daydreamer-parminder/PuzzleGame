using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScore : MonoBehaviour
{
    public TMP_Text
        levelText,
        turnText, 
        matchesText;

    public void DisplayData(int level, int turns, int matches)
    {
        levelText.SetText(level.ToString());
        turnText.SetText(turns.ToString());
        matchesText.SetText(matches.ToString());
    }
}
