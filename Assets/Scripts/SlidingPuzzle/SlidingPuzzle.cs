
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SlidingPuzzle : MonoBehaviour
{
    // Countdown timer properties
    public float initialTime = 300f;
    private float currentTime = 0f;
    [HideInInspector]
    public bool isCountdownRunning = false;
    [HideInInspector]
    public bool timesOut = false;

    // TMP_Text to display countdown and events
    public TMP_Text[] countdownText;
    public FadeScreen fadeScreen;

    // Events for countdown callbacks
    public delegate void CountdownStart();
    public event CountdownStart OnCountdownStart;

    public delegate void CountdownComplete();
    public event CountdownComplete OnCountdownComplete;

    // Sliding puzzle properties
    public GameObject[] tiles;
    public Transform emptySpace;
    private Dictionary<Vector2Int, GameObject> tilePositions = new();
    private Vector3[] originalPosition;
    private int gridSize;

    public delegate void PuzzleSolved();
    public event PuzzleSolved OnPuzzleSolved;

    void OnEnable()
    {
        InitializePuzzle();
        // Initialize the countdown timer
        currentTime = initialTime;
        UpdateCountdownDisplay();
        isCountdownRunning = false;
        if (fadeScreen != null)
            fadeScreen.OnFadeaOutComplete += StartCountdown;
        else
            StartCountdown();
        OnCountdownComplete += StopCountdown;
    }

    void InitializePuzzle()
    {
        // Set up the grid size and original positions
        gridSize = (int)Mathf.Sqrt(tiles.Length + 1);
        originalPosition = new Vector3[tiles.Length + 1];
        tilePositions = new Dictionary<Vector2Int, GameObject>();

        // Store the original positions
        for (int i = 0; i < tiles.Length; i++)
        {
            //Vector2Int pos = new(i % gridSize, i / gridSize);
            originalPosition[i] = tiles[i].transform.position;
        }


        // Save the original position of the empty space
        originalPosition[tiles.Length] = emptySpace.position;

        // Collect all tiles and add a null for the empty space to shuffle
        List<GameObject> allTiles = tiles.ToList();
        allTiles.Add(null); // Add null to represent the empty space
        Shuffle(allTiles);

        // After shuffling, assign each tile to a new grid position
        for (int i = 0; i < allTiles.Count; i++)
        {
            // Calculate the grid position for each tile in the shuffled list
            Vector2Int pos = new(i % gridSize, i / gridSize);
            GameObject tile = allTiles[i];

            // Assign the tile or empty space to this position in the dictionary
            tilePositions[pos] = tile;

            // Position the tile or the empty space in the 3D world
            if (tile != null)
            {
                tile.transform.position = originalPosition[i];
            }
            else
            {
                emptySpace.position = originalPosition[i];
            }
        }
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(0, list.Count);
            (list[randomIndex], list[i]) = (list[i], list[randomIndex]);
        }
    }

    public void TryMoveTile(TileInteractable tile, Vector2Int tilePos)
    {
        Vector2Int emptyPos = GetEmptySpacePosition();

        // Check if the tile is directly adjacent in either x or y axis, but not both
        bool isDirectlyAdjacent = (Mathf.Abs(tilePos.x - emptyPos.x) == 1 && tilePos.y == emptyPos.y) || (Mathf.Abs(tilePos.y - emptyPos.y) == 1 && tilePos.x == emptyPos.x);

        // Check if tile is adjacent to the empty space
        if (isDirectlyAdjacent)
        {
            // Swap tile position with empty space
            tilePositions[emptyPos] = tile.gameObject;
            tilePositions[tilePos] = null;

            // Move the tile to the empty space position and update empty space position
            (emptySpace.position, tile.transform.position) = (tile.transform.position, emptySpace.position);

            // Check if puzzle is solved
            if (IsPuzzleSolved())
            {
                OnPuzzleSolvedFunction();
            }
        }
    }

    public Vector2Int GetEmptySpacePosition()
    {
        foreach (var entry in tilePositions)
        {
            if (entry.Value == null)
                return entry.Key;
        }
        return Vector2Int.zero; // Default fallback
    }

    public Vector2Int GetTilePosition(GameObject tile)
    {
        foreach (var entry in tilePositions)
        {
            if (entry.Value == tile)
                return entry.Key;
        }
        return Vector2Int.zero;
    }

    private bool IsPuzzleSolved()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            // Determine the correct position for each tile in a solved puzzle
            Vector2Int correctPosition = new(i % gridSize, i / gridSize);

            // Check if the tile at this position in the dictionary matches the expected tile
            if (tilePositions[correctPosition] != tiles[i])
                return false;
        }

        // Ensure the empty space is in the correct final position
        Vector2Int emptySpaceFinalPosition = new(gridSize - 1, gridSize - 1);
        if (tilePositions[emptySpaceFinalPosition] != null)
            return false;

        return true;
    }

    private void OnPuzzleSolvedFunction()
    {
        Debug.Log("Puzzle Solved!");
        OnPuzzleSolved?.Invoke();
    }

    public void CompletePuzzleDirectly()
    {
        // Set up the grid size and correct positions
        gridSize = (int)Mathf.Sqrt(tiles.Length + 1);
        tilePositions = new Dictionary<Vector2Int, GameObject>();

        // Arrange each tile in its correct position
        for (int i = 0; i < tiles.Length; i++)
        {
            Vector2Int correctPosition = new(i % gridSize, i / gridSize);
            tilePositions[correctPosition] = tiles[i];

            // Move tile to its correct world position
            tiles[i].transform.position = originalPosition[i];
        }

        // Position the empty space at the last cell
        Vector2Int emptySpaceFinalPosition = new(gridSize - 1, gridSize - 1);
        tilePositions[emptySpaceFinalPosition] = null;
        emptySpace.position = originalPosition[tiles.Length];

        // Trigger the puzzle solved event
        OnPuzzleSolvedFunction();
    }

    private void OnDestroy()
    {
        if (fadeScreen != null)
            fadeScreen.OnFadeaOutComplete -= StartCountdown;
    }

    void Update()
    {
        if (IsPuzzleSolved())
        {
            isCountdownRunning = false;
            return;
        }

        if (isCountdownRunning)
        {
            currentTime -= Time.deltaTime;

            if (currentTime <= 0)
            {
                currentTime = 0;
                isCountdownRunning = false;
                timesOut = true;
                OnCountdownComplete?.Invoke();
            }
            UpdateCountdownDisplay();
        }
    }

    public void StartCountdown()
    {
        if (!isCountdownRunning)
        {
            isCountdownRunning = true;
            timesOut = false;
            OnCountdownStart?.Invoke();
        }
    }

    public void StopCountdown()
    {
        isCountdownRunning = false;
        SceneTransitionManager.singleton.GoToScene(1);
    }

    private void UpdateCountdownDisplay()
    {
        if (countdownText != null)
        {
            foreach (var item in countdownText)
            {
                item.text = FormatTime(currentTime);
            }
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
