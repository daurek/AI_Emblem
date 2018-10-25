using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{

    #region UI

    public GameObject hoverInfo;
    private GameObject tileInfo;
    private GameObject unitInfo;
    private TextMeshProUGUI tileName;
    private TextMeshProUGUI tileBonus;
    private TextMeshProUGUI unitName;
    private TextMeshProUGUI unitStats;

    #endregion

    private Tile hoveredTile;

    private void Awake()
    {
        hoverInfo.SetActive(false);
        tileInfo = hoverInfo.transform.GetChild(0).gameObject;
        unitInfo = hoverInfo.transform.GetChild(1).gameObject;

        tileName = tileInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tileBonus = tileInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        unitName = unitInfo.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        unitStats = unitInfo.transform.GetChild(1).GetComponent<TextMeshProUGUI>();


        tileName.text = "";
        tileBonus.text = "";
        unitName.text = "";
        unitStats.text = "";
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 1, LayerMask.GetMask("Tiles"));
        if (hit)
        {
            Tile newTile = hit.collider.GetComponent<Tile>();
            if (hoveredTile != newTile)
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

        if (hoveredTile)
        {
            if(!hoverInfo.activeSelf) hoverInfo.SetActive(true);
            hoverInfo.transform.position = Input.mousePosition;
        }
    }

    public void SetHoverInfo()
    {   
        tileName.text = hoveredTile.TileData.tileName;
        if (hoveredTile.TileData.bonusUnit) tileBonus.SetText("+ " + hoveredTile.TileData.bonusDamage + "<sprite=1> to <color=yellow><b>" + hoveredTile.TileData.bonusUnit.unitName + "</b>");
        else tileBonus.text = "";

        if (hoveredTile.currentUnit)
        {
            unitName.text = hoveredTile.currentUnit.unitData.unitName;


            unitStats.SetText
                (
                 "<sprite=0> " + hoveredTile.currentUnit.CurrentHealth / hoveredTile.currentUnit.unitData.maxHealth

                );
        }
        else unitName.text = "";
    }
}
