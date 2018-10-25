using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;
    public int CurrentDamage { get; private set; }
    public int CurrentHealth { get; private set; }
    private bool hasAttacked;
    private bool hasMoved;

    private void Awake()
    {
        CurrentDamage = unitData.baseDamage;
        CurrentHealth = unitData.maxHealth;
    }
   


}
