using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public int 
        cols, 
        rows, 
        pairs;
    public Cell cellPrefab;
    public Card cardPrefab;
    public CardData[] cardSymbols;
    public float 
        padding, 
        hidingTime;
}
