using System.Collections;
using UnityEngine;

/// <summary>
/// Handles unit current stats and moving
/// </summary>
public class Unit : MonoBehaviour
{
    public UnitData UnitData            { get; private set; }
    public int CurrentDamage            { get; private set; }
    public int CurrentHealth            { get; set; }
    public int CurrentMovementPoints    { get; private set; }
    public bool HasAttacked             { get; set; }
    public bool IsMoving                { get; set; }
    public bool hasTurn                 { get; set; }
    public bool IsDead                  { get; private set; }
    public int Player                   { get; set; }
    public Tile CurrentTile             { get; set; }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetLayer(1);
    }

    /// <summary>
    /// Sets the unit data along with it's current data
    /// </summary>
    /// <param name="_unitData"></param>
    public void SetUnitData(UnitData _unitData)
    {
        UnitData = _unitData;
        CurrentDamage = UnitData.baseDamage;
        CurrentHealth = UnitData.maxHealth;
        CurrentMovementPoints = UnitData.movementSpeed;
        GetComponent<SpriteRenderer>().sprite = UnitData.unitSprite;
    }
   
    /// <summary>
    /// Moves unit to the destination and attacks the unit if it has been given
    /// </summary>
    /// <param name="destination"></param>
    /// <param name="otherUnit"></param>
    /// <returns></returns>
    public IEnumerator Move(Tile destination, Unit otherUnit = null)
    {   
        // Move handles movement in two ways
        // if A-->B is a straight line then it goes directly
        // if A-->B is a diagonal line then it gets divided into two lines


        IsMoving = true;
        Tile oldTile = CurrentTile;
        CurrentTile = destination;
        destination.currentUnit = this;
        oldTile.currentUnit = null;

        // Get the distance in straight line
        CurrentMovementPoints -= (int)GameManager.DistanceWithLines(transform.position, destination.Position);
        Selector.instance.MovingUnit = true;

        Vector2 finalPosition;

        // Check if the unit can go on a straight line
        if (transform.position.x != destination.Position.x && transform.position.y != destination.Position.y)
        {
            finalPosition = new Vector2(transform.position.x, destination.Position.y);

            while ((Vector2)transform.position != finalPosition)
            {
                transform.position = Vector2.MoveTowards(transform.position, finalPosition, UnitData.movementSpeed * Time.deltaTime);
                yield return new WaitForEndOfFrame();
            }
        }

        finalPosition = destination.Position;

        while ((Vector2)transform.position != finalPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, finalPosition, UnitData.movementSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        destination.currentUnit = this;

        // Buff the unit when it arrives if the bonus tile is correct
        if (destination.TileData.bonusUnit && destination.TileData.bonusUnit.unitName == destination.currentUnit.UnitData.unitName)
            CurrentDamage = UnitData.baseDamage + destination.TileData.bonusDamage;
        else
            CurrentDamage = UnitData.baseDamage;

        // Update information
        Selector.instance.SetSelectedInfo();
        Selector.instance.MovingUnit = false;
        IsMoving = false;

        // Hit only if it has been given an unit
        if (otherUnit)
        {
            otherUnit.Hit(CurrentDamage);
            HasAttacked = true;
            Used();
        }

        hasTurn = false;
    }

    /// <summary>
    /// Hits this unit with the given damage and kills it if it's health reaches zero
    /// </summary>
    /// <param name="damage"></param>
    public void Hit(int damage)
    {
        CurrentHealth -= damage;
        Selector.instance.CreateDamageText(damage, CurrentTile.currentUnit.transform.position);
        if (CurrentHealth <= 0)
        {
            IsDead = true;
            Selector.instance.Log("<color=orange> " + UnitData.unitName + " has died \n");
            GameManager.instance.RemoveUnit(this, Player);
            Selector.instance.SetHoverInfo();
        }
    }

    /// <summary>
    /// Updates unit layer
    /// </summary>
    /// <param name="layer"></param>
    public void SetLayer(int layer)
    {
        spriteRenderer.sortingOrder = layer;
    }

    /// <summary>
    /// Resets unit state to it can move again
    /// </summary>
    public void ResetUnit()
    {
        HasAttacked = false;
        CurrentMovementPoints = UnitData.movementSpeed;
        spriteRenderer.color = Color.white;
    }

    /// <summary>
    /// Sets unit to used so it can't attack again
    /// </summary>
    public void Used()
    {
        spriteRenderer.color = Color.red;
    }

}
