using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum GameState 
{
    NotStarted,
    Started,
    Result
}

public delegate void MatchEvent(int level, int turns, int matches);

public class Gameplay : MonoBehaviour, IClickListener
{
    [Header("Static Parms")]
    [SerializeField]
    protected CardData[] cardsData;
    [SerializeField]
    protected LevelData[] levels;
    [SerializeField]
    protected GridLayoutGroup gridLayoutGroup;
    [SerializeField]
    protected UIScore uiScore;
    [SerializeField]
    protected UserDataPrefs userData;
    [SerializeField]
    protected CellPool cellPool;
    [SerializeField]
    protected CardPool cardPool;

    [Header("Progression Parms")]
    [SerializeField]
    protected GameState gameState = GameState.NotStarted;
    [SerializeField]
    protected LevelData currentLevel;
    protected Cell[,] gridBoard;
    [SerializeField]
    protected List<Card> playable, matched;

    [Header("Progression Parms")]
    public UnityEvent OnGameEndsCallBack;

    [Header("Debugging Parms")]
    public bool isForcedLevel = false;
    public int forcedLevel = 0;

    private void Awake() 
    {
        userData.matchEvent = uiScore.DisplayData;
        cellPool.Create();
        cardPool.Create();
    }

    public void StartGame() 
    {
        playable = new List<Card>();
        matched = new List<Card>();
        userData.LoadData();
        if (isForcedLevel)
        {
            LoadeLevel(levels[forcedLevel]);
            SetUpCells();
            MakePairsAndAssignCardsToGrid();
            Invoke("HideAllCards", currentLevel.hidingTime);
        }
        else
        if (userData.inGame == 1)
        {
            LoadeLevel(levels[userData.level - 1]);
            SetUpCells();
            SetPreviousGameState();
        }
        else
        {
            LoadeLevel(levels[userData.level - 1]);
            SetUpCells();
            MakePairsAndAssignCardsToGrid();
            Invoke("HideAllCards", currentLevel.hidingTime);
        }
    }

    public virtual void LoadeLevel(LevelData level) => currentLevel = level;

    public void SetUpCells() 
    {
        // make cells
        gridBoard = new Cell[currentLevel.cols, currentLevel.rows];

        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = currentLevel.cols;
        gridLayoutGroup.spacing = new Vector2(currentLevel.padding, currentLevel.padding);

        for (int y = 0, ind = 0; y < currentLevel.rows; y++)
        {
            for (int x = 0; x < currentLevel.cols; x++, ++ind)
            {
                gridBoard[x, y] = cellPool.Pick();
                gridBoard[x, y].transform.SetParent(gridLayoutGroup.transform);
                gridBoard[x, y].index = ind;
            }
        }
    }

    public void MakePairsAndAssignCardsToGrid() 
    {
        // make pairs

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

        // make cards on cells wrt to pairs

        List<CardData> cardsD = new List<CardData>(currentLevel.cardSymbols);
        for (int i = 0; i < pairsList.Count; ++i) 
        {
            int sel = Random.Range(0, cardsD.Count);
            for (int j = 0; j < pairsList[i].Length; ++j) 
            {
                Vector2Int cord = IndexToCords(pairsList[i][j] + 1, currentLevel);
                Card prefabGO = cardPool.Pick();
                prefabGO.transform.SetParent(gridBoard[cord.x - 1, cord.y - 1].transform);
                prefabGO.transform.localPosition = Vector3.zero;
                gridBoard[cord.x - 1, cord.y - 1].SetCard(prefabGO);
                prefabGO.SetData(cardsD[sel]);
                prefabGO.SetPlayable(true);
                prefabGO.SetListener(this);
                prefabGO.Reveal();
                playable.Add(prefabGO);
            }
            cardsD.RemoveAt(sel);
        }
    }

    public void SetPreviousGameState()
    {
        string[] strArr = userData.gameState.Split(',');
        int[] tarr = new int[strArr.Length - 1];
        for (int i = 0; i < tarr.Length; ++i) 
            tarr[i] = int.Parse(strArr[i]);
        for (int y = 0, ind = 0; y < currentLevel.rows; y++)
        {
            for (int x = 0; x < currentLevel.cols; x++, ++ind)
            {
                // make card in cell if was present
                if (tarr[ind] != -1)
                {
                    Card prefabGO = cardPool.Pick();
                    prefabGO.transform.SetParent(gridBoard[x, y].transform);
                    prefabGO.transform.localPosition = Vector3.zero;

                    gridBoard[x, y].SetCard(prefabGO);
                    prefabGO.SetData(cardsData[tarr[ind]]);
                    prefabGO.SetPlayable(true);
                    prefabGO.SetListener(this);
                    playable.Add(prefabGO);
                }
            }
        }
        gameState = GameState.Started;
    }

    public void OnClicked(IClickable clickable) 
    {
        if (gameState == GameState.Started) 
        {
            Card crdclicked = clickable as Card;
            if (matched.Count > 0)
            {
                if (matched[matched.Count - 1].cardData.type == crdclicked.cardData.type)
                {
                    matched.Add(crdclicked);
                    crdclicked.SetPlayable(false);
                    if (matched.Count >= currentLevel.pairs)
                    {
                        foreach (var crd in matched)
                        {
                            playable.Remove(crd);
                            crd.SetPlayable(false);
                            cardPool.Add(crd);
                        }
                        matched.Clear();
                        userData.IncreaseTurn();
                        userData.IncreaseScore(1);
                        if (playable.Count == 0)
                        {
                            userData.LevelUp();
                            gameState = GameState.Result;
                            userData.SetGameState(false, "");
                            OnGameEndsCallBack.Invoke();
                        }
                        userData.LoadData();
                    }
                }
                else
                {
                    userData.IncreaseTurn();
                    foreach (var crd in matched)
                        crd.SetPlayable(true);
                    matched.Clear();
                    HideAllCards();
                    userData.LoadData();
                }
            }
            else
            {
                matched.Add(crdclicked);
                crdclicked.SetPlayable(false);
            }
        }
    }

    public string ParseGridToString() 
    {
        string ans = "";

        if (gridBoard != null)
        {
            for (int row = 0; row < currentLevel.rows; row++)
            {
                for (int col = 0; col < currentLevel.cols; col++)
                {
                    Cell cell = gridBoard[col, row];
                    if (cell != null && cell.GetCard() != null)
                    {
                        // Append the type of the card to the string
                        ans += cell.GetCard().IsPlayable() ? cell.GetCard().cardData.type.ToString() : -1;
                    }
                    else
                    {
                        // Append a placeholder for an empty cell
                        ans += "-1";
                    }

                    // Add a delimiter between cells
                    ans += ",";
                }

                // Add a newline character between rows
                ans += "\n";
            }
        }

        return ans;
    }

    public void HideAllCards()
    {
        for (int x = 0; x < currentLevel.cols; ++x)
        {
            for (int y = 0; y < currentLevel.rows; ++y)
            {
                if (gridBoard[x, y].GetCard() != null && gridBoard[x, y].GetCard().IsPlayable())
                    gridBoard[x, y].GetCard().Hide();
            }
        }
        gameState = GameState.Started;
    }

    public void ResetGame()
    {
        foreach (var itm in matched)
            cardPool.Add(itm);
        foreach (var itm in playable)
            cardPool.Add(itm);
        matched.Clear();
        playable.Clear();
        for (int x = 0; x < currentLevel.cols; ++x)
            for (int y = 0; y < currentLevel.rows; ++y)
                cellPool.Add(gridBoard[x, y]);
        gameState = GameState.NotStarted;
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

    public void OnApplicationQuit()
    {
        if (gameState == GameState.Started)
        {
            userData.SetGameState(true, ParseGridToString());
        }
    }
}
