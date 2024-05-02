using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public int 
        cols, 
        rows, 
        pairs;
    public CardData[] cardSymbols;
    public float 
        padding, 
        hidingTime,
        comboTime;
}
