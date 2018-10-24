using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData unitData;
    private int currentDamage;
    private int currentHealth;
    private bool hasAttacked;
    private bool hasMoved;

    private void Awake()
    {
        currentDamage = unitData.baseDamage;
        currentHealth = unitData.maxHealth;
    }
   


}
