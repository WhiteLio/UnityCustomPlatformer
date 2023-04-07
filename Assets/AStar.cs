using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum Employee
{
    S,
    A,
    L,
    G,
}

public class AStar : MonoBehaviour
{
    public Tilemap tilemap;
    public Tilemap tilemapRoads;
    public Tilemap tilemapBuildings;
    public LineRenderer lineRenderer;
    public Employee employee;
    
    private readonly Vector3Int START = new Vector3Int(-26, -5, 0);
    private readonly Vector3Int GOAL = new Vector3Int(16, 13, 0);

    void Start()
    {
        PlotPath(Search());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print(GetTileUnderMouse());
        }
    }

    public Vector3Int GetTileUnderMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int tilePos = tilemap.WorldToCell(mouseWorldPos);
        return tilePos;
    }

    public List<Vector3Int> Search()
    {
        // Create a list of open tiles and add the start tile
        // Note: PriorityQueue would be faster but it is bugged in Unity :<
        List<Tile> openTiles = new List<Tile>();
        openTiles.Add(new Tile(START, 0, 0, 0, null));

        // Create a dictionary of closed tiles
        Dictionary<Vector3Int, Tile> closedTiles = new Dictionary<Vector3Int, Tile>();

        // Keep searching while there are open tiles
        while (openTiles.Count > 0)
        {
            // Get the tile with the lowest f value
            Tile currentTile = openTiles.OrderBy(t => t.f).First();

            // Reached the goal
            if (currentTile.position == GOAL)
            {
                // Reconstruct path
                List<Vector3Int> path = new List<Vector3Int>();
                while (currentTile != null)
                {
                    path.Add(currentTile.position);
                    currentTile = currentTile.parent;
                }
                path.Reverse();
                return path;
            }

            // Move the current tile from open to closed
            openTiles.Remove(currentTile);
            closedTiles[currentTile.position] = currentTile;

            // Loop through the neighbors of the current tile
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    // Skip the current tile itself
                    if (x == 0 && y == 0) { continue; }

                    // Initialise the neighbour tile
                    Vector3Int neighborPosition = currentTile.position + new Vector3Int(x, y, 0);
                    float h = GetH(neighborPosition);
                    float g = currentTile.g + GetDiagonalDistance(currentTile.position, neighborPosition);
                    float f = g + h;
                    Tile neighbor = new Tile(neighborPosition, f, g, h, currentTile);
                    
                    // Check if tile is already open
                    Tile alreadyOpenedTile = openTiles.FirstOrDefault(t => t.position == neighborPosition);
                    if (alreadyOpenedTile != null)
                    {
                        if (neighbor.g >= alreadyOpenedTile.g)
                        {
                            // Skip if it is not better
                            continue;
                        }
                        else
                        {
                            // Replace it if it is better
                            openTiles.Remove(alreadyOpenedTile);
                        }
                    }
                    
                    // Skip if tile is already closed
                    bool alreadyClosed = closedTiles.ContainsKey(neighborPosition);
                    if (alreadyClosed)
                    {
                        continue;
                    }

                    // Add tile to open tiles
                    openTiles.Add(neighbor);
                }
            }
        }

        // There is no path to the goal
        Debug.LogWarning("No path found");
        return null;
    }

    public float GetH(Vector3Int tilePosition)
    {
        float h = 0;
        float distanceCost = GetDiagonalDistance(tilePosition, GOAL);
        h += distanceCost;

        switch (employee)
        {
            case Employee.S:
                // Implement shadeCost here
                break;
            case Employee.A:
                float roadCost = tilemapRoads.HasTile(tilePosition) ? 1 : 0;
                h += roadCost;
                break;
            case Employee.L:
                float buildingCost = tilemapBuildings.HasTile(tilePosition) ? 1 : 0;
                h += buildingCost;
                break;
            case Employee.G:
                // Implement rainCost here
                break;
            default:
                break;
        }

        return h;
    }

    public float GetDiagonalDistance(Vector3Int a, Vector3Int b)
    {
        float dx = Mathf.Abs(a.x - b.x);
        float dy = Mathf.Abs(a.y - b.y);
        
        return (dx + dy) + (Mathf.Sqrt(2) - 2) * Mathf.Min(dx, dy);
    }

    public void PlotPath(List<Vector3Int> path)
    {
        // Set the line renderer's position count
        lineRenderer.positionCount = path.Count;

        // Loop through the path and set each position
        for (int i = 0; i < path.Count; i++)
        {
            lineRenderer.SetPosition(i, path[i] + new Vector3(0.5f, 0.5f, 0f));
        }
    }
}

public class Tile
{
    public Vector3Int position;
    public float f;
    public float g;
    public float h;
    public Tile parent;
    
    public Tile(Vector3Int position, float f, float g, float h, Tile parent)
    {
        this.position = position;
        this.f = f;
        this.g = g;
        this.h = h;
        this.parent = parent;
    }

    public override string ToString()
    {
        return position + " f:" + f + " g:" + g + " h:" + h + " parent:" + parent.position;
    }
}