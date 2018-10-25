using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData TileData { get; private set; }
    public Unit currentUnit;
    private Vector2 position;

    public void SetTileData(TileData _tileData)
    {
        TileData = _tileData;
        GetComponent<SpriteRenderer>().color = TileData.tileSprite;
    }
}

