using UnityEngine;

/// <summary>
/// Data Class that stores information about a tile and if it buffs an unit
/// </summary>
[CreateAssetMenu(fileName = "TileData")]
public class TileData : ScriptableObject
{
	public string tileName;
    public int bonusDamage;
    public UnitData bonusUnit;
    public Color tileSprite;
}

/// <summary>
/// Auxiliary class that stores a row of data
/// </summary>
[System.Serializable]
public class TileDataRow
{
    public DataMap[] dataMap;
}

/// <summary>
/// Data that stores information on a tile, it's unit and the player side 
/// </summary>
[System.Serializable]
public class DataMap
{
    public TileData tileData;
    public UnitData unitData;
    [Range(0,1)] public int playerId;
}
