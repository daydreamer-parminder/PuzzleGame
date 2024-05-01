using UnityEngine;

public abstract class AUserData : MonoBehaviour
{
    public int level;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        LoadData();
    }

    public abstract void LoadData();
    public abstract void SaveScore();
    public abstract void SaveLevel();

    // method to increase score
    public void IncreaseScore(int amount)
    {
        score += amount;
        SaveScore(); // Save score after increasing
    }

    // method to level up
    public void LevelUp()
    {
        level++;
        SaveLevel(); // Save level after leveling up
    }
}
