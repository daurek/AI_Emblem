using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TileDataRow[] tileMapData;
    public UnitData unitExample;
    public GameObject squarePrefab;

    private bool gameOver;

    public int PlayerTurn { get; set; }         // 0 => Player1 ---- 1 => Player2
    public List<Unit>[] Player;
    [SerializeField] private TMPro.TextMeshProUGUI playerText;
   
    public List<List<Tile>> tileMap;

    private Transform levelContainer;
    private Transform unitsContainer;

    private List<GameObject> rangeIndicatorList = new List<GameObject>();

    public Color ableToMove;
    public Color enemy;
    public Color ally;

    private void Awake()
    {
        instance = this;

        levelContainer = new GameObject().transform;
        levelContainer.name = "Level Container";
        unitsContainer = new GameObject().transform;
        unitsContainer.name = "Units Container";

        Player      = new List<Unit>[2];
        Player[0]   = new List<Unit>();
        Player[1]   = new List<Unit>();

        playerText.text = "Turn: Player" + (PlayerTurn+1);

        tileMap = new List<List<Tile>>();

        CreateMap();

        CreateUnits();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump")) ChangeTurn();
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
                if (i == 2 || i == 6)
                {
                    Unit newUnit = Instantiate(squarePrefab, tileMap[i][j].transform.position, Quaternion.identity, unitsContainer).AddComponent<Unit>();
                    Player[i / 5].Add(newUnit);
                    newUnit.Player = i / 5;
                    newUnit.SetUnitData(unitExample);
                    tileMap[i][j].currentUnit = newUnit;
                    newUnit.CurrentTile = tileMap[i][j];
                }
            }
        }
    }

    public void UnitRangeIndicator(Tile currentTile)
    {
        currentTile.currentUnit.SetLayer(2);

        int movementRange = currentTile.currentUnit.CurrentMovementPoints;
        int attackRange = currentTile.currentUnit.UnitData.range;
        int totalRange= 0;
        int range = 0;

        if (movementRange < attackRange)
        {
            range = attackRange;

        }
        else
        {
            range = movementRange;
        }

        totalRange = range * 2 + 1;
        

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

                    Tile tileRef = tileMap[(int)rangePos.y][(int)rangePos.x];

                    if (distance <= attackRange && tileRef.currentUnit && !currentTile.currentUnit.HasAttacked)
                    {
                        SpriteRenderer rangeRenderer = Instantiate(squarePrefab, tileRef.Position, Quaternion.identity, tileRef.transform).GetComponent<SpriteRenderer>();
                        print(currentTile.currentUnit.Player + " " + tileRef.currentUnit.Player);
                        if (currentTile.currentUnit.Player != tileRef.currentUnit.Player)
                            rangeRenderer.color = enemy;
                        else
                            rangeRenderer.color = ally;

                        rangeRenderer.sortingOrder = 1;
                        rangeIndicatorList.Add(rangeRenderer.gameObject);
                        
                    }
                    else if (distance <= movementRange && !tileRef.currentUnit)
                    {
                        SpriteRenderer rangeRenderer = Instantiate(squarePrefab, tileRef.Position, Quaternion.identity, tileRef.transform).GetComponent<SpriteRenderer>();
                        rangeRenderer.sortingOrder = 1;
                        rangeRenderer.color = ableToMove;
                        rangeIndicatorList.Add(rangeRenderer.gameObject);
                    }
                }
            }
        }        

    }

    public List<Tile> UnitRangeIndicatorAI(Tile currentTile)
    {
        int movementRange = currentTile.currentUnit.CurrentMovementPoints;
        int attackRange = currentTile.currentUnit.UnitData.range;
        int totalRange = 0;
        int range = 0;
        List<Tile> possiblesTilesToMove = new List<Tile>();

        if (movementRange < attackRange)
        {
            range = attackRange;
        }
        else
        {
            range = movementRange;
        }

        totalRange = range * 2 + 1;


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

                    Tile tileRef = tileMap[(int)rangePos.y][(int)rangePos.x];

                    //if (distance <= attackRange && tileRef.currentUnit && !currentTile.currentUnit.HasAttacked)
                    //{
                    //    SpriteRenderer rangeRenderer = Instantiate(squarePrefab, tileRef.Position, Quaternion.identity, tileRef.transform).GetComponent<SpriteRenderer>();
                    //    print(currentTile.currentUnit.Player + " " + tileRef.currentUnit.Player);
                    //    if (currentTile.currentUnit.Player != tileRef.currentUnit.Player)
                    //        rangeRenderer.color = enemy;
                    //    else
                    //        rangeRenderer.color = ally;

                    //    rangeRenderer.sortingOrder = 1;
                    //    rangeIndicatorList.Add(rangeRenderer.gameObject);

                    //}
                    if (distance <= movementRange && !tileRef.currentUnit)
                    {
                        possiblesTilesToMove.Add(tileRef);
                    }
                }
            }
        }
        return possiblesTilesToMove;
    }

    public List<Tile> UnitRangeIndicatorAIAttack(Tile currentTile, Unit currentUnit)
    {
        int movementRange = currentUnit.CurrentMovementPoints;
        int attackRange = currentUnit.UnitData.range;
        int totalRange = 0;
        int range = 0;
        List<Tile> possiblesTilesToAttack = new List<Tile>();

        if (movementRange < attackRange)
        {
            range = attackRange;
        }
        else
        {
            range = movementRange;
        }

        totalRange = range * 2 + 1;


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

                    Tile tileRef = tileMap[(int)rangePos.y][(int)rangePos.x];

                    if (distance <= attackRange && tileRef.currentUnit && !currentUnit.HasAttacked)
                    {
                        if (currentUnit.Player != tileRef.currentUnit.Player)
                            possiblesTilesToAttack.Add(tileRef);
                    }
                }
            }
        }
        return possiblesTilesToAttack;
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

    public void RemoveUnit(Unit remove, int player)
    {
        Player[player].Remove(remove);
        Destroy(remove.gameObject);
        if (Player[player].Count == 0)
        {
            gameOver = true;
            Selector.instance.Log("<color=green> Player" + (PlayerTurn + 1) + " wins \n");
        }
    }


    public void ChangeTurn()
    {
        if (!gameOver)
        {
            ClearRangeIndicator();
            PlayerTurn = (PlayerTurn + 1 )% 2;
            playerText.text = "Turn: Player" + (PlayerTurn + 1);
            foreach (List<Unit> list in Player)
            {
                foreach (Unit unit in list)
                {
                    unit.ResetUnit();
                }
            }

            if (PlayerTurn == 1)
            {
                IA.instance.Play();
            }
            
        }
    }
}


