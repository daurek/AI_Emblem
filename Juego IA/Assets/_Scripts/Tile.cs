using UnityEngine;

/// <summary>
/// Tile that stores the tiledata information and if it has a unit on it
/// </summary>
public class Tile : MonoBehaviour
{   
    public TileData TileData { get; private set; }
    /// <summary>
    /// Unit that the tile currently has
    /// </summary>
    public Unit currentUnit;
    public Vector2 Position { get; private set; }
    public int tileId { get; set; }

    /// <summary>
    /// Sets tile information after being created
    /// </summary>
    /// <param name="_tileData"></param>
    /// <param name="position"></param>
    public void SetTileData(TileData _tileData, Vector2 position)
    {
        TileData = _tileData;
        Position = position;
        GetComponent<SpriteRenderer>().color = TileData.tileSprite;
    }
}

