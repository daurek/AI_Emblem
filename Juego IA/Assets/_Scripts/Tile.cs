using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileData TileData { get; private set; }
    public Unit currentUnit;
    public Vector2 Position { get; private set; }
    public int tileId { get; set; }

    public void SetTileData(TileData _tileData, Vector2 position)
    {
        TileData = _tileData;
        Position = position;
        GetComponent<SpriteRenderer>().color = TileData.tileSprite;
    }
}

