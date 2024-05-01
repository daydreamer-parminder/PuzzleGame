using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    // static data
    [SerializeField]
    protected Cell cellPrefab;
    [SerializeField]
    protected LevelData[] levels;
    [SerializeField]
    protected Transform 
        cardsLayoutParent, 
        startPoint;

    // progression data
    [SerializeField]
    protected LevelData currentLevel;
    protected Cell[,] gridBoard;
    [SerializeField]
    protected AUserData userData;
    public bool isForcedLevel = false;
    public int forcedLevel = 0;

    // Start is called before the first frame update
    private void Start()
    {
        userData.LoadData();
        if(isForcedLevel)
            LoadeLevel(levels[forcedLevel]);
        else
            LoadeLevel(levels[userData.level - 1]);
    }

    public virtual void LoadeLevel(LevelData level) 
    {
        currentLevel = level;
        gridBoard = new Cell[currentLevel.cols, currentLevel.rows];

        float cardWidth = currentLevel.cardPrefab.GetComponent<RectTransform>().rect.width;
        float cardHeight = currentLevel.cardPrefab.GetComponent<RectTransform>().rect.height;
        for (int y = 0, ind = 1; y < currentLevel.rows; y++)
        {
            for (int x = 0; x < currentLevel.cols; x++, ++ind)
            {
                gridBoard[x, y] = Instantiate(cellPrefab,
                        startPoint.position + // starting point
                        new Vector3(cardWidth * x, -cardHeight * y, 0) +  // card post
                        new Vector3(x > 0 ? currentLevel.padding * x : 0, y > 0 ? -currentLevel.padding * y : 0, 0), // padding
                        Quaternion.identity,
                        cardsLayoutParent);
                gridBoard[x, y].index = ind;

                Card prefabGO = Instantiate(currentLevel.cardPrefab, gridBoard[x, y].transform);
                prefabGO.transform.localPosition = Vector3.zero;

                gridBoard[x, y].SetCard(prefabGO);
            }
        }
        SetCardsOnGrid();
        Invoke("HideAllCards", currentLevel.hidingTime);
    }

    public void SetCardsOnGrid() 
    {
        int ttlCells = currentLevel.rows * currentLevel.cols;
        int possiblePairs = ttlCells / currentLevel.pairs;

        List<int> indexes = new List<int>();
        for (int i = 0; i < ttlCells; ++i)
            indexes.Add(i);

        List<int[]> pairsList = new List<int[]>();
        for (int i = 0; i < possiblePairs; ++i) 
        {
            int[] pairs = new int[currentLevel.pairs];
            for (int j = 0; j < currentLevel.pairs; ++j) 
            {
                int sel = Random.Range(0, indexes.Count);
                pairs[j] = indexes[sel];
                indexes.RemoveAt(sel);
            }
            pairsList.Add(pairs);
        }

        List<CardData> cardsD = new List<CardData>(currentLevel.cardSymbols);
        for (int i = 0; i < pairsList.Count; ++i) 
        {
            int sel = Random.Range(0, cardsD.Count);
            for (int j = 0; j < pairsList[i].Length; ++j) 
            {
                Vector2Int cord = IndexToCords(pairsList[i][j] + 1, currentLevel);
                gridBoard[cord.x - 1, cord.y - 1].GetCard().SetData(cardsD[sel]);
                gridBoard[cord.x - 1, cord.y - 1].GetCard().SetPlayable(true);
                gridBoard[cord.x - 1, cord.y - 1].GetCard().Reveal();
            }
            cardsD.RemoveAt(sel);
        }

        for (int i = 0; i < indexes.Count; ++i)
        {
            Vector2Int cord = IndexToCords(indexes[i] + 1, currentLevel);
            gridBoard[cord.x - 1, cord.y - 1].gameObject.SetActive(false);
        }
    }

    public Vector2Int IndexToCords(int index, LevelData level) 
    {
        int rem = index % level.cols;
        int divnum = index - rem;
        if (index > 0 && index % level.cols == 0) 
            return new Vector2Int(level.cols, divnum / level.cols);
        else
            return new Vector2Int(index % level.cols, (divnum / level.cols) + 1);
    }

    public void HideAllCards() 
    {
        for (int x = 0; x < currentLevel.cols; ++x) 
        {
            for (int y = 0; y < currentLevel.rows; ++y)
            {
                if(gridBoard[x, y].GetCard().IsPlayable())
                   gridBoard[x, y].GetCard().Hide();
            }
        }
    }

    public void ClearGrid() 
    {
        
    }
}
