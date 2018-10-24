using UnityEngine;

public class Tile : MonoBehaviour
{
    private TileData tileData;
    private Unit currentUnit;
    private Vector2 position;

    public void SetTileData(TileData _tileData)
    {
        tileData = _tileData;
        GetComponent<SpriteRenderer>().color = tileData.tileSprite;
    }
}

