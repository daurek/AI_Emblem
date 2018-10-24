using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileDataRow[] tileMapData;
    public GameObject tilePrefab;
    private Tile[,] tileMap;
    private Transform levelContainer;

    private void Awake()
    {
        levelContainer = new GameObject().transform;
        levelContainer.name = "Level Container";

        for (int i = 0; i < tileMapData.Length; i++)
        {
            for (int j = 0; j < tileMapData[i].tileDataRow.Length; j++)
            {
                Tile newTile = Instantiate(tilePrefab, new Vector2(j, i), Quaternion.identity, levelContainer).GetComponent<Tile>();
                newTile.SetTileData(tileMapData[i].tileDataRow[j]);
            }
        }
    }
}
