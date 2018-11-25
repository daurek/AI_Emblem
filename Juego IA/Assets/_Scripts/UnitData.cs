using UnityEngine;

/// <summary>
/// Data Class that stores information about the unit
/// </summary>
[CreateAssetMenu(fileName = "UnitData")]
public class UnitData : ScriptableObject
{
    public string   unitName;
    public int      id;
    public int      baseDamage;
    public int      range;
    public int      movementSpeed;
    public int      maxHealth;
    public Sprite   unitSprite;  
}
