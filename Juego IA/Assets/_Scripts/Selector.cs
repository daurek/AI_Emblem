using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles user selection and interface
/// </summary>
public class Selector : MonoBehaviour
{
    public static Selector instance;

    #region UI

    public TextMeshProUGUI logRef;
    public GameObject hoverInfo;
    public GameObject selectInfo;
    public GameObject damageTextPrefab;
    private RectTransform canvasRect;
    private GameObject tileInfo;
    private GameObject unitInfo;
    private GameObject unitStatsInfo;
    private TextMeshProUGUI tileName;
    private TextMeshProUGUI tileBonus;
    private TextMeshProUGUI unitName;
    private TextMeshProUGUI unitStats;
    private TextMeshProUGUI selectUnitDamageText;
    private TextMeshProUGUI selectUnitSpeedText;
    private TextMeshProUGUI selectUnitRangeText;

    private TextMeshProUGUI selectedUnitName;
    private TextMeshProUGUI selectedUnitHealthCount;
    private Image selectedUnitImage;
    private Image selectedHealthBar;

    #endregion

    /// <summary>
    /// Tile that user is currently hovering
    /// </summary>
    private Tile hoveredTile;
    /// <summary>
    /// Tile that user has selected
    /// </summary>
    private Tile selectedTile;
    /// <summary>
    /// Previous tile reference
    /// </summary>
    private Tile oldTile;
    /// <summary>
    /// Unit that user has selected
    /// </summary>
    private Unit selectedUnit;

    /// <summary>
    /// If an unit is moving when don't let the user do anything
    /// </summary>
    public bool MovingUnit { get; set; }

    private void Awake()
    {
        instance = this;

        #region Getting UI References

        canvasRect = GetComponent<RectTransform>();
        hoverInfo.SetActive(false);
        selectInfo.SetActive(false);

        tileInfo = hoverInfo.transform.GetChild(0).gameObject;
        unitInfo = hoverInfo.transform.GetChild(1).gameObject;
        unitStatsInfo = selectInfo.transform.GetChild(4).gameObject;

        tileName = tileInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tileBonus = tileInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        unitName = unitInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        unitStats = unitInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        selectedUnitImage = selectInfo.transform.GetChild(0).GetComponent<Image>();
        selectedUnitName = selectInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        selectedHealthBar = selectInfo.transform.GetChild(2).GetChild(0).GetComponent<Image>();
        selectedUnitHealthCount = selectInfo.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>();

        selectUnitDamageText = unitStatsInfo.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        selectUnitSpeedText = unitStatsInfo.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        selectUnitRangeText = unitStatsInfo.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();

        #endregion

        // Reseting texts
        tileName.text = "";
        tileBonus.text = "";
        unitName.text = "";
        unitStats.text = "";
        selectedUnitName.text = "";
    }

    private void Update()
    {   
        // Raycast to check if user is hovering a tile
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, LayerMask.GetMask("Tiles"));
        if (hit)
        {
            Tile newTile = hit.collider.GetComponent<Tile>();

            if(!hoveredTile || hoveredTile.tileId != newTile.tileId)
            {
                hoveredTile = newTile;
                SetHoverInfo();
            }
        }
        else
        {
            hoveredTile = null;
            if (hoverInfo.activeSelf) hoverInfo.SetActive(false);
        }

        // Update hover info position to mouse
        if (hoveredTile)
        {
            if(!hoverInfo.activeSelf) hoverInfo.SetActive(true);
            hoverInfo.transform.position = Input.mousePosition;
        }
        
        // Select
        if (Input.GetMouseButtonDown(0))
        {
            if (!MovingUnit)
            {
                // If tile selected has unit then select that unit
                if (hoveredTile && hoveredTile.currentUnit && hoveredTile.currentUnit.Player == GameManager.instance.PlayerTurn && !hoveredTile.currentUnit.HasAttacked)
                {   
                    if(selectedUnit) selectedUnit.SetLayer(1);

                    GameManager.instance.ClearRangeIndicator();
                    selectedUnit = hoveredTile.currentUnit;
                    oldTile = hoveredTile;
                    SetSelectedInfo();
                    selectInfo.SetActive(true);
                    GameManager.instance.UnitRangeIndicator(oldTile);
                }
                // If not then clear the range indicator
                else
                {
                    GameManager.instance.ClearRangeIndicator();
                    selectedUnit = null;
                    selectInfo.SetActive(false);
                }
            }
        }

        // Move or Attack
        if (Input.GetMouseButtonDown(1))
        {   
            // No unit is moving
            if (!MovingUnit)
            {   
                // Unit is selected
                if (selectedUnit)
                {
                    selectedTile = hoveredTile;

                    // Destination tile has been selected
                    if (selectedTile)
                    {   
                        // Destination tile is in range of movement
                        if (selectedTile.transform.childCount != 0)
                        {   
                            // No unit on that tile
                            if (!selectedTile.currentUnit)
                            {   
                                // Move unit
                                GameManager.instance.ClearRangeIndicator();
                                selectedUnit.SetLayer(1);
                                oldTile.currentUnit = null;
                                StartCoroutine(selectedUnit.Move(selectedTile));
                                Log("<color=white> " + selectedUnit.UnitData.name + " (Player" + (selectedUnit.Player + 1) + ") moved to<color=yellow> " + selectedTile.TileData.tileName + " (" + (selectedTile.Position.x+1) + "," + (selectedTile.Position.y + 1)+ ")\n");
                                SetSelectedInfo();
                                oldTile = selectedTile;
                            }
                            // Unit on tile
                            else
                            {
                                if (!oldTile.currentUnit.HasAttacked)
                                {
                                    // On range
                                    if (selectedUnit.UnitData.range >= GameManager.DistanceWithLines(selectedUnit.transform.position, selectedTile.currentUnit.transform.position))
                                    {
                                        if(oldTile.currentUnit.Player != selectedTile.currentUnit.Player)
                                        {
                                            // Attack
                                            Log("<color=white> " + selectedUnit.UnitData.name +" (Player" +(selectedUnit.Player+1) + ") dealt <color=red>" + selectedUnit.CurrentDamage + "<color=white> damage to " + selectedTile.currentUnit.UnitData.name + "\n");
                                            GameManager.instance.ClearRangeIndicator();
                                            selectedUnit.SetLayer(1);
                                            selectedTile.currentUnit.Hit(selectedUnit.CurrentDamage);                                            
                                            SetHoverInfo();
                                            oldTile.currentUnit.HasAttacked = true;
                                            oldTile.currentUnit.Used();
                                        }
                                        else
                                        {
                                            Log("<color=orange> " + selectedUnit.UnitData.unitName + " is your ally \n");
                                        }
                                    }
                                    else
                                    {
                                        Log("<color=red> " + selectedUnit.UnitData.unitName + " is out of range \n");
                                    }
                                }                                      
                            }
                        }                        
                    }

                    
                }
            }
            
        }
    }

    /// <summary>
    /// Sets the information of the hovered unit and/or tile close to the mouse
    /// </summary>
    public void SetHoverInfo()
    {
        if (hoveredTile)
        {
            unitName.text = "";
            unitStats.text = "";
            tileBonus.text = "";

            tileName.text = hoveredTile.TileData.tileName;
            if (hoveredTile.TileData.bonusUnit) tileBonus.SetText("+ " + hoveredTile.TileData.bonusDamage + "<sprite name=Damage> to <color=yellow><b>" + hoveredTile.TileData.bonusUnit.unitName + "</b>");

            if (hoveredTile.currentUnit && !hoveredTile.currentUnit.IsDead)
            {
                unitName.text = hoveredTile.currentUnit.UnitData.unitName + " Player" + (hoveredTile.currentUnit.Player + 1);

                unitStats.SetText
                    (
                     "<sprite name=Health> " + hoveredTile.currentUnit.CurrentHealth + "/" + hoveredTile.currentUnit.UnitData.maxHealth +   "\n" +
                     "<sprite name=Damage> " + hoveredTile.currentUnit.CurrentDamage +                                                      "\n" +
                     "<sprite name=Speed> "  + hoveredTile.currentUnit.UnitData.movementSpeed +                                             "\n" +
                     "<sprite name=Range> "  + hoveredTile.currentUnit.UnitData.range +                                                     "\n"
                    );
            }

            Canvas.ForceUpdateCanvases();
            hoverInfo.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
        }
    }

    /// <summary>
    /// Sets the information of the selected unit to the down right corner panel
    /// </summary>
    public void SetSelectedInfo()
    {
        if (selectedUnit)
        {
            selectedUnitName.text = selectedUnit.UnitData.unitName;
            selectedUnitImage.sprite = selectedUnit.UnitData.unitSprite;

            selectedHealthBar.fillAmount = (float)selectedUnit.CurrentHealth / selectedUnit.UnitData.maxHealth;
            selectedUnitHealthCount.text = selectedUnit.CurrentHealth + " / " + selectedUnit.UnitData.maxHealth;
            selectUnitDamageText.text = "" + selectedUnit.CurrentDamage;
            selectUnitSpeedText.text = selectedUnit.CurrentMovementPoints + " / " + selectedUnit.UnitData.movementSpeed;
            selectUnitRangeText.text = "" + selectedUnit.UnitData.range;
        }
    }

    /// <summary>
    /// Creates a damage text above a position
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="position"></param>
    public void CreateDamageText(int damage, Vector2 position)
    {   
        // Set that position to a canvas position 
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(position);
        Vector2 WorldObject_ScreenPosition = new Vector2(
        ((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
        ((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

        // Create text and fill it with damage
        RectTransform ui = Instantiate(damageTextPrefab, transform).GetComponent<RectTransform>();
        ui.GetComponent<TextMeshProUGUI>().text = ""  + damage;
        ui.anchoredPosition = WorldObject_ScreenPosition;
    }

    /// <summary>
    /// Logs out a text
    /// </summary>
    /// <param name="logMessage"></param>
    public void Log(string logMessage)
    {
        logRef.SetText(logRef.text + logMessage);
    }
}
