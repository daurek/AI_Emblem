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
    public DataMap[] dataMap;
}

[System.Serializable]
public class DataMap
{
    public TileData tileData;
    public UnitData unitData;
    [Range(0,1)] public int playerId;
}
