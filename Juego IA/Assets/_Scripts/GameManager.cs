using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles game management, turn, wining condition
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public TileDataRow[] tileMapData;
    public UnitData unitExample;
    public GameObject squarePrefab;
    public GameObject selectionPrefab;

    public int playerCount = 5;

    private bool gameOver;

    public int PlayerTurn { get; set; }         // 0 => Player1 ---- 1 => Player2
    public List<Unit>[] Player;
    [SerializeField] private TMPro.TextMeshProUGUI playerText;
   
    public List<List<Tile>> tileMap;

    // Containers for the units and tiles
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
    }

    private void Start()
    {   
        CreateMap();
    }

    private void Update()
    {   
        // If it's your turn and you press space then the turn changes to the AI
        if (PlayerTurn == 0 && Input.GetButtonDown("Jump")) ChangeTurn();
    }

    /// <summary>
    /// Creates the map with the given data on the inspector
    /// </summary>
    private void CreateMap()
    {
        int idCounter = 0;

        // Loops through tile map data
        for (int i = 0; i < tileMapData.Length; i++)
        {
            List<Tile> tempList = new List<Tile>();
            for (int j = 0; j < tileMapData[i].dataMap.Length; j++)
            {   
                // Creates tile and sets its data
                Tile newTile = Instantiate(squarePrefab, new Vector2(j, i), Quaternion.identity, levelContainer).AddComponent<Tile>();
                newTile.gameObject.AddComponent<BoxCollider2D>();
                newTile.SetTileData(tileMapData[i].dataMap[j].tileData, new Vector2(j, i));
                newTile.tileId = idCounter++;

                // If it has a unit
                if (tileMapData[i].dataMap[j].unitData)
                {   
                    // Create the unit
                    Unit newUnit = Instantiate(squarePrefab, newTile.Position, Quaternion.identity, unitsContainer).AddComponent<Unit>();
                    newUnit.Player = tileMapData[i].dataMap[j].playerId;
                    // Adds unit to the player
                    Player[newUnit.Player].Add(newUnit);
                    // Sets data to the unit
                    newUnit.SetUnitData(tileMapData[i].dataMap[j].unitData);
                    newTile.currentUnit = newUnit;
                    newUnit.CurrentTile = newTile;
                }
                tempList.Add(newTile);
            }
            tileMap.Add(tempList);
        }
    }

    /// <summary>
    /// Sets visually the units to it's respective colors
    /// </summary>
    /// <param name="currentTile"></param>
    public void UnitRangeIndicator(Tile currentTile)
    {
        currentTile.currentUnit.SetLayer(2);

        int movementRange = currentTile.currentUnit.CurrentMovementPoints;
        int attackRange = currentTile.currentUnit.UnitData.range;
        int totalRange= 0;
        int range = 0;

        // Gets the higher range
        if (movementRange < attackRange)
        {
            range = attackRange;
        }
        else
        {
            range = movementRange;
        }

        totalRange = range * 2 + 1;
        
        // Creates array for the unit's range
        GameObject[,] rangeArr = new GameObject[totalRange, totalRange];
        
        for (int i = 0; i < rangeArr.GetLength(0); i++)
        {
            for (int j = 0; j < rangeArr.GetLength(1); j++)
            {
                Vector2 rangePos = new Vector2(currentTile.Position.x + i - range, currentTile.Position.y + j - range);
                
                // If range isn't leaving the tilemap
                if (rangePos.x >= 0 && rangePos.y >= 0 && rangePos.x < tileMap[0].Count && rangePos.y < tileMap[0].Count)
                {
                    // Calculate distance to point
                    float distance = DistanceWithLines(currentTile.Position, rangePos);

                    // If distance inside the range
                    if (distance <= range)
                    {
                        Tile tileRef = tileMap[(int)rangePos.y][(int)rangePos.x];
                        
                        // If it's not the current unit tile
                        if (tileRef != currentTile)
                        {
                            // If the distance is inferior to the attack range and it has an unit 
                            if (distance <= attackRange && tileRef.currentUnit)
                            {   
                                SpriteRenderer rangeRenderer = Instantiate(selectionPrefab, tileRef.Position, Quaternion.identity, tileRef.transform).GetComponent<SpriteRenderer>();
                                rangeRenderer.sortingOrder = 1;
                                // If it's an enemy unit then set a Red selection around them units
                                if (currentTile.currentUnit.Player != tileRef.currentUnit.Player)
                                    rangeRenderer.color = enemy;
                                // If it's an allied unit then set a Blue selection around them units
                                else
                                    rangeRenderer.color = ally;

                                rangeIndicatorList.Add(rangeRenderer.gameObject);
                            }
                            // distance lower than the movement range and doesnt have an unit
                            else if (distance <= movementRange && !tileRef.currentUnit)
                            {   
                                SpriteRenderer rangeRenderer = Instantiate(selectionPrefab, tileRef.Position, Quaternion.identity, tileRef.transform).GetComponent<SpriteRenderer>();
                                rangeRenderer.sortingOrder = 1;
                                rangeRenderer.color = ableToMove;
                                rangeIndicatorList.Add(rangeRenderer.gameObject);
                            }
                            
                        }
                        // White selection around the selected unit
                        else
                        {
                            SpriteRenderer rangeRenderer = Instantiate(selectionPrefab, tileRef.Position, Quaternion.identity, tileRef.transform).GetComponent<SpriteRenderer>();
                            rangeRenderer.sortingOrder = 1;
                            rangeIndicatorList.Add(rangeRenderer.gameObject);
                        }
                    }
                }
            }
        }        
    }

    /// <summary>
    /// Return a list of tiles that the AI can move into
    /// </summary>
    /// <param name="currentTile"></param>
    /// <returns></returns>
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

                if (rangePos.x >= 0 && rangePos.y >= 0 && rangePos.x < tileMap[0].Count && rangePos.y < tileMap[0].Count)
                {
                    // Calculate distance to point
                    float distance = DistanceWithLines(currentTile.Position, rangePos);

                    Tile tileRef = tileMap[(int)rangePos.y][(int)rangePos.x];

                    if (distance <= movementRange && (!tileRef.currentUnit || tileRef.currentUnit == currentTile.currentUnit))
                    {
                        possiblesTilesToMove.Add(tileRef);
                    }
                }
            }
        }
        return possiblesTilesToMove;
    }

    /// <summary>
    /// Returns a list of the possible tiles that the unit can attack
    /// </summary>
    /// <param name="currentTile"></param>
    /// <param name="currentUnit"></param>
    /// <returns></returns>
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

                if (rangePos.x >= 0 && rangePos.y >= 0 && rangePos.x < tileMap[0].Count && rangePos.y < tileMap[0].Count)
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

    /// <summary>
    /// Clears the range indicator (only visual)
    /// </summary>
    public void ClearRangeIndicator()
    {
        for (int i = 0; i < rangeIndicatorList.Count; i++)
        {
            Destroy(rangeIndicatorList[i]);
        }
        rangeIndicatorList.Clear();

    }

    /// <summary>
    /// Returns distance between two points
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Removes unit from the player's array
    /// </summary>
    /// <param name="unitToRemove"></param>
    /// <param name="player"></param>
    public void RemoveUnit(Unit unitToRemove, int player)
    {
        Player[player].Remove(unitToRemove);
        Destroy(unitToRemove.gameObject);
        if (Player[player].Count == 0)
        {
            gameOver = true;
            Selector.instance.Log("<color=green> Player" + (PlayerTurn + 1) + " wins \n");
        }
    }

    /// <summary>
    /// Changes turn to the other player
    /// </summary>
    public void ChangeTurn()
    {
        if (!gameOver)
        {
            ClearRangeIndicator();
            PlayerTurn = (PlayerTurn + 1 )% 2;
            playerText.text = "Turn: Player" + (PlayerTurn + 1);
            // Reset every unit state
            foreach (List<Unit> list in Player)
            {
                foreach (Unit unit in list)
                {
                    unit.ResetUnit();
                }
            }

            // Play AI only it the turn has reached 1
            if (PlayerTurn == 1)
            {
                IA.instance.Play();
            }
            
        }
    }

}


