
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public static Maze Instance { get; private set; }
    public MazeSetting[] settingArray;
    [HideInInspector]
    public MazeSetting setting = null;
    [HideInInspector]
    public int currentSettingIndex = 0;
    [HideInInspector]
    public Vector3[,] gridList;
    [HideInInspector]
    public Vector3 gridScale;
    [HideInInspector]
    public List<GameObject>[,] gridWallList;

    [HideInInspector]
    public Camera mapCamera;

    [HideInInspector]
    public Vector3 startPosition;
    [HideInInspector]
    public Vector3 endPosition;
    [HideInInspector]
    public Vector3 randomPosition;
    [HideInInspector]
    public Vector3 destroyedWallPosition;

    public Material transparentMaterial;
    public UIObjectInteraction UIObjectInteraction;

    void Start()
    {
        setting = settingArray[currentSettingIndex];
        GenerateMaze();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateMaze()
    {
        ResetMaze();
        Random.InitState(setting.seed);
        gridList = new Vector3[setting.row, setting.column];
        gridWallList = new List<GameObject>[setting.row, setting.column];
        CreateGrid();
        GeneratePrimMaze();
        SetupUpStartingPointAndEndingPoint();
        SetupCamera();
    }

    // To get any position in the maze
    public Vector3 GetRandomValidPosition()
    {
        int randomRow = Random.Range(0, setting.row);
        int randomCol = Random.Range(0, setting.column);
        Vector3 enemyPosition = gridList[randomRow, randomCol];
        enemyPosition.x += (gridScale.x / 2);
        enemyPosition.z -= (gridScale.z / 2);
        return enemyPosition;
    }

    void ResetMaze()
    {
        if (gridWallList != null)
        {
            for (int i = 0; i < gridWallList.GetLength(0); i++)
            {
                for (int j = 0; j < gridWallList.GetLength(1); j++)
                {
                    List<GameObject> wallDestroy = gridWallList[i, j];
                    if (wallDestroy != null)
                    {
                        foreach (GameObject wall in wallDestroy)
                        {
                            if (wall != null)
                                Destroy(wall);
                        }
                        wallDestroy.Clear();
                    }
                }
            }
        }
        GameObject floor = GameObject.Find("MazeFloor");
        if (floor != null)
        {
            Destroy(floor);
        }
        if (mapCamera != null)
        {
            Destroy(mapCamera.gameObject);
        }
        mapCamera = null;

    }

    void CreateGrid()
    {
        // Generate the floor
        GameObject floor = Instantiate(setting.floorPrefab, transform);
        floor.name = "MazeFloor";
        floor.transform.localScale = new Vector3(setting.width, 0.1f, setting.height);

        // Generate the grid from the top left corner of the floor
        Vector3 tempGridPosition = floor.transform.position;
        tempGridPosition.x -= (setting.width / 2);
        tempGridPosition.y += floor.transform.localScale.y;
        tempGridPosition.z += (setting.height / 2);

        // The position of the grid is the top left corner coordinate
        Vector3 gridPosition = tempGridPosition;
        gridScale = new Vector3(setting.width / setting.column, 1f, setting.height / setting.row);
        for (int row = 0; row < setting.row; row++)
        {
            for (int col = 0; col < setting.column; col++)
            {
                gridList[row, col] = gridPosition;
                gridPosition.x += gridScale.x;
            }
            gridPosition.z -= gridScale.z;
            gridPosition.x = tempGridPosition.x;
        }

        // Initiate the wall for the four side of the grid
        for (int row = 0; row < setting.row; row++)
        {
            for (int col = 0; col < setting.column; col++)
            {
                Vector3 wallPos = gridList[row, col];
                List<GameObject> wallList = new();
                // clockwise to set up the wall
                // wall up
                GameObject wallUp = Instantiate(setting.wallPrefab, wallPos, Quaternion.identity, transform);
                wallUp.transform.localScale = new Vector3(gridScale.x, setting.depth, gridScale.z / 5);
                wallUp.transform.position = new Vector3(wallPos.x + (wallUp.transform.localScale.x / 2), wallPos.y + (setting.depth / 2), wallPos.z);
                wallUp.name = string.Format("({0},{1}) Wall Up", row, col);
                wallList.Add(wallUp);

                // wall right
                wallPos.x += gridScale.x;
                GameObject wallRight = Instantiate(setting.wallPrefab, wallPos, Quaternion.identity, transform);
                wallRight.transform.localScale = new Vector3(gridScale.x / 5, setting.depth, gridScale.z);
                wallRight.transform.position = new Vector3(wallPos.x, wallPos.y + (setting.depth / 2), wallPos.z - (wallRight.transform.localScale.z / 2));
                wallRight.name = string.Format("({0},{1}) Wall Right", row, col);
                wallList.Add(wallRight);

                // wall down
                wallPos.z -= gridScale.z;
                GameObject wallDown = Instantiate(setting.wallPrefab, wallPos, Quaternion.identity, transform);
                wallDown.transform.localScale = new Vector3(gridScale.x, setting.depth, gridScale.z / 5);
                wallDown.transform.position = new Vector3(wallPos.x - (wallDown.transform.localScale.x / 2), wallPos.y + (setting.depth / 2), wallPos.z);
                wallDown.name = string.Format("({0},{1}) Wall Down", row, col);
                wallList.Add(wallDown);

                // wall left
                wallPos.x -= gridScale.x;
                GameObject wallLeft = Instantiate(setting.wallPrefab, wallPos, Quaternion.identity, transform);
                wallLeft.transform.localScale = new Vector3(gridScale.x / 5, setting.depth, gridScale.z);
                wallLeft.transform.position = new Vector3(wallPos.x, wallPos.y + (setting.depth / 2), wallPos.z + (wallLeft.transform.localScale.z / 2));
                wallLeft.name = string.Format("({0},{1}) Wall Left", row, col);
                wallList.Add(wallLeft);
                gridWallList[row, col] = wallList;
            }
        }
    } // end of CreateGrid

    private void GeneratePrimMaze()
    {
        Vector2Int startingCell = new(Random.Range(0, setting.row - 1), Random.Range(0, setting.column - 1));
        List<Vector2Int> visitedCells = new();
        visitedCells.Add(startingCell);
        List<Vector2Int> frontierCells = new();
        AddUnvisitedNeighboringCellsToFrontier(startingCell, visitedCells, frontierCells);

        // Continue until the frontier list is empty
        while (frontierCells.Count > 0)
        {
            int randomIndex = Random.Range(0, frontierCells.Count - 1);
            Vector2Int currentCell = frontierCells[randomIndex];
            frontierCells.RemoveAt(randomIndex);
            Vector2Int neighbourCell = GetRandomVisitedNeighborCell(currentCell, visitedCells);
            CreatePassageway(currentCell, neighbourCell);
            visitedCells.Add(currentCell);
            AddUnvisitedNeighboringCellsToFrontier(currentCell, visitedCells, frontierCells);
        }
    }

    private void AddUnvisitedNeighboringCellsToFrontier(Vector2Int cell, List<Vector2Int> visitedCells, List<Vector2Int> frontierCells)
    {
        // cell above
        if (cell.x - 1 >= 0 && !visitedCells.Contains(new Vector2Int(cell.x - 1, cell.y)) && !frontierCells.Contains(new Vector2Int(cell.x - 1, cell.y)))
            frontierCells.Add(new Vector2Int(cell.x - 1, cell.y));

        // cell right
        if (cell.y + 1 < setting.column && !visitedCells.Contains(new Vector2Int(cell.x, cell.y + 1)) && !frontierCells.Contains(new Vector2Int(cell.x, cell.y + 1)))
            frontierCells.Add(new Vector2Int(cell.x, cell.y + 1));

        // cell below
        if (cell.x + 1 < setting.row && !visitedCells.Contains(new Vector2Int(cell.x + 1, cell.y)) && !frontierCells.Contains(new Vector2Int(cell.x + 1, cell.y)))
            frontierCells.Add(new Vector2Int(cell.x + 1, cell.y));

        // cell left
        if (cell.y - 1 >= 0 && !visitedCells.Contains(new Vector2Int(cell.x, cell.y - 1)) && !frontierCells.Contains(new Vector2Int(cell.x, cell.y - 1)))
            frontierCells.Add(new Vector2Int(cell.x, cell.y - 1));
    }

    private Vector2Int GetRandomVisitedNeighborCell(Vector2Int cell, List<Vector2Int> visitedCells)
    {
        List<Vector2Int> visitedNeighbours = new();

        // cell above
        if (cell.x - 1 >= 0 && visitedCells.Contains(new Vector2Int(cell.x - 1, cell.y)))
            visitedNeighbours.Add(new Vector2Int(cell.x - 1, cell.y));

        // cell right
        if (cell.y + 1 < setting.column && visitedCells.Contains(new Vector2Int(cell.x, cell.y + 1)))
            visitedNeighbours.Add(new Vector2Int(cell.x, cell.y + 1));

        // cell below
        if (cell.x + 1 < setting.row && visitedCells.Contains(new Vector2Int(cell.x + 1, cell.y)))
            visitedNeighbours.Add(new Vector2Int(cell.x + 1, cell.y));

        // cell left
        if (cell.y - 1 >= 0 && visitedCells.Contains(new Vector2Int(cell.x, cell.y - 1)))
            visitedNeighbours.Add(new Vector2Int(cell.x, cell.y - 1));

        // the cell cannot find any visited cell
        if (visitedNeighbours.Count == 0) return Vector2Int.zero;

        int randomIndex = Random.Range(0, visitedNeighbours.Count - 1);
        return visitedNeighbours[randomIndex];
    }

    private void CreatePassageway(Vector2Int cell1, Vector2Int cell2)
    {
        // Determine the orientation of the wall
        if (cell1.x == cell2.x) // same row
        {
            if (cell1.y < cell2.y) // cell1 at left, cell2 at right
            {
                DestroyWall(cell1, 1);
                DestroyWall(cell2, 3);
            }
            else
            {
                DestroyWall(cell1, 3);
                DestroyWall(cell2, 1);
            }
        }
        else // same column
        {
            if (cell1.x < cell2.x) // cell1 at above, cell2 at below
            {
                DestroyWall(cell1, 2);
                DestroyWall(cell2, 0);
            }
            else
            {
                DestroyWall(cell1, 0);
                DestroyWall(cell2, 2);
            }
        }
    }

    private void SetupUpStartingPointAndEndingPoint()
    {
        Vector2Int start = Vector2Int.zero;
        Vector2Int end = Vector2Int.zero;
        switch (setting.startingSide)
        {
            case MazeSetting.WallFrom.Up:
                start.y = setting.startingIndex - 1;
                setting.startWall = gridWallList[start.x, start.y][0];
                destroyedWallPosition = setting.startWall.transform.localPosition;
                InvisbleWall(start, 0);
                break;
            case MazeSetting.WallFrom.Down:
                start.x = setting.row - 1;
                start.y = setting.startingIndex - 1;
                setting.startWall = gridWallList[start.x, start.y][2];
                destroyedWallPosition = setting.startWall.transform.localPosition;
                InvisbleWall(start, 2);
                break;
            case MazeSetting.WallFrom.Left:
                start.x = setting.startingIndex - 1;
                setting.startWall = gridWallList[start.x, start.y][3];
                destroyedWallPosition = setting.startWall.transform.localPosition;
                InvisbleWall(start, 3);
                break;
            case MazeSetting.WallFrom.Right:
                start.x = setting.startingIndex - 1;
                start.y = setting.column - 1;
                setting.startWall = gridWallList[start.x, start.y][1];
                destroyedWallPosition = setting.startWall.transform.localPosition;
                InvisbleWall(start, 1);
                break;
        }
        switch (setting.endingSide)
        {
            case MazeSetting.WallFrom.Up:
                end.y = setting.endingIndex - 1;
                InvisbleEndWall(end, 0);
                break;
            case MazeSetting.WallFrom.Down:
                end.x = setting.row - 1;
                end.y = setting.endingIndex - 1;
                InvisbleEndWall(end, 2);
                break;
            case MazeSetting.WallFrom.Left:
                end.x = setting.endingIndex - 1;
                InvisbleEndWall(end, 3);
                break;
            case MazeSetting.WallFrom.Right:
                end.x = setting.endingIndex - 1;
                end.y = setting.column - 1;
                InvisbleEndWall(end, 1);
                break;
        }
        setting.startingPoint = start;
        setting.endingPoint = end;

        Vector3 gridStart = gridList[start.x, start.y];
        gridStart.x += (gridScale.x / 2);
        gridStart.z -= (gridScale.z / 2);
        startPosition = gridStart;

        Vector3 gridEnd = gridList[end.x, end.y];
        gridEnd.x += (gridScale.x / 2);
        gridEnd.z -= (gridScale.z / 2);
        endPosition = gridEnd;
    }

    void DestroyWall(Vector2Int grid, int side)
    {
        GameObject wallDestroy;
        wallDestroy = gridWallList[grid.x, grid.y][side];
        gridWallList[grid.x, grid.y][side] = null;
        Destroy(wallDestroy);
    }

    void InvisbleWall(Vector2Int grid, int side)
    {
        GameObject invisbleWall;
        invisbleWall = gridWallList[grid.x, grid.y][side];
        invisbleWall.GetComponent<Renderer>().material = transparentMaterial;
    }
    void InvisbleEndWall(Vector2Int grid, int side)
    {
        GameObject endWall;
        endWall = gridWallList[grid.x, grid.y][side];
        endWall.GetComponent<Renderer>().material = transparentMaterial;
        endWall.GetComponent<BoxCollider>().isTrigger = true;
        endWall.name = "EndWall";
    }

    private void SetupCamera()
    {

        // Create a camera for rendering the map view
        mapCamera = new GameObject("MapCamera").AddComponent<Camera>();
        Camera setupCam = mapCamera.GetComponent<Camera>();
        setupCam.clearFlags = CameraClearFlags.SolidColor;
        setupCam.orthographic = true;
        setupCam.farClipPlane = 100;
        setupCam.nearClipPlane = 1f;
        setupCam.orthographicSize = Mathf.Max(setting.width, setting.height) / 2;
        setupCam.depth = -1;
        setupCam.targetTexture = setting.mapRender;

        float cameraHeight = Mathf.Max(setting.height, setting.width) + 5;  // Adjust the height as needed.

        mapCamera.transform.parent = transform;
        mapCamera.transform.localPosition = new Vector3(0, cameraHeight, 0);
        mapCamera.transform.localRotation = Quaternion.Euler(90, 0, 0);

        // Ensure the camera is active
        mapCamera.gameObject.SetActive(true);
    }
}
