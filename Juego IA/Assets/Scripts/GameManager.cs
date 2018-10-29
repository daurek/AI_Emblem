using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public TileDataRow[] tileMapData;
    public UnitData unitExample;
    public GameObject squarePrefab;
    private List<List<Tile>> tileMap;

    private Transform levelContainer;
    private Transform unitsContainer;

    private List<GameObject> rangeIndicatorList = new List<GameObject>();

    public Color ableToMove;
    public Color blocked;

    private void Awake()
    {
        instance = this;


        levelContainer = new GameObject().transform;
        levelContainer.name = "Level Container";
        unitsContainer = new GameObject().transform;
        unitsContainer.name = "Units Container";

        tileMap = new List<List<Tile>>();

        CreateMap();

        CreateUnits();
       


    }

    private void CreateMap()
    {
        int idCounter = 0;
        for (int i = 0; i < tileMapData.Length; i++)
        {
            List<Tile> tempList = new List<Tile>();
            for (int j = 0; j < tileMapData[i].tileDataRow.Length; j++)
            {
                Tile newTile = Instantiate(squarePrefab, new Vector2(j, i), Quaternion.identity, levelContainer).AddComponent<Tile>();
                newTile.gameObject.AddComponent<BoxCollider2D>();
                newTile.SetTileData(tileMapData[i].tileDataRow[j], new Vector2(j, i));
                newTile.tileId = idCounter++;
                tempList.Add(newTile);
            }
            tileMap.Add(tempList);
        }
    }

    private void CreateUnits()
    {
        for (int i = 0; i < tileMap.Count; i++)
        {
            for (int j = 0; j < tileMap[i].Count; j++)
            {
                if (i == 5)
                {
                    Unit newUnit = Instantiate(squarePrefab, tileMap[i][j].transform.position, Quaternion.identity, unitsContainer).AddComponent<Unit>();
                    newUnit.SetUnitData(unitExample);
                    tileMap[i][j].currentUnit = newUnit;

                }
                
            }
        }
    }

    public void UnitRangeIndicator(Tile currentTile)
    {
        bool onlyShowEnemies = false;

        currentTile.currentUnit.SetLayer(2);

        int range = currentTile.currentUnit.CurrentMovementPoints;
        int totalRange= 0;

        if (range == 0)
        {
            onlyShowEnemies = true;
            range = currentTile.currentUnit.UnitData.range; 
        }
        
        totalRange = range * 2 + 1;

        print(range);
        GameObject[,] rangeArr = new GameObject[totalRange, totalRange];
        
        for (int i = 0; i < rangeArr.GetLength(0); i++)
        {
            for (int j = 0; j < rangeArr.GetLength(1); j++)
            {
                Vector2 rangePos = new Vector2(currentTile.Position.x + i - range, currentTile.Position.y + j - range);
                
                if (rangePos.x >= 0 && rangePos.y >= 0 && rangePos.x < tileMap[0].Count && rangePos.y < tileMap[0].Count && tileMap[(int)rangePos.y][(int)rangePos.x] != currentTile)
                {
                    
                    // Calculate distance to point
                    float distance = DistanceWithLines(currentTile.Position, rangePos);

                    if (distance <= range)
                    {
                        Tile tileRef = tileMap[(int)rangePos.y][(int)rangePos.x];

                        if (!onlyShowEnemies)
                        {
                            SpriteRenderer rangeRenderer = Instantiate(squarePrefab, tileRef.Position, Quaternion.identity, tileRef.transform).GetComponent<SpriteRenderer>();
                            rangeRenderer.sortingOrder = 1;
                            if (tileRef.currentUnit) rangeRenderer.color = blocked;
                            else rangeRenderer.color = ableToMove;
                            rangeIndicatorList.Add(rangeRenderer.gameObject);
                        }
                        else if(tileRef.currentUnit)
                        {
                            SpriteRenderer rangeRenderer = Instantiate(squarePrefab, tileRef.Position, Quaternion.identity, tileRef.transform).GetComponent<SpriteRenderer>();
                            rangeRenderer.sortingOrder = 1;
                            rangeRenderer.color = blocked;
                            rangeIndicatorList.Add(rangeRenderer.gameObject);
                        }
                    }
                }
            }
        }
        

    }

    public void ClearRangeIndicator()
    {
        for (int i = 0; i < rangeIndicatorList.Count; i++)
        {
            Destroy(rangeIndicatorList[i]);
        }
        rangeIndicatorList.Clear();

    }

    public static float DistanceWithLines(Vector2 start, Vector2 end)
    {
        float distance;
        if (start.x != end.x && start.y != end.y)
        {
            // Not straight line
            Vector2 midPoint = new Vector2(start.x , end.y);
            distance = Vector2.Distance(start, midPoint) + Vector2.Distance(midPoint, end);
        }
        else
        {
            // Straight line
            distance = Vector2.Distance(start, end);
        }
        return distance;
    }
}
