using UnityEngine;

[CreateAssetMenu(fileName = "TileData")]
public class TileData : ScriptableObject
{

	public string tileName;
    public int bonusDamage;
    public UnitData bonusUnit;
    public Color tileSprite;

}

[System.Serializable]
public class TileDataRow
{
    public TileData[] tileDataRow;
}
