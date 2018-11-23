using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData UnitData { get; private set; }
    public int CurrentDamage { get; private set; }
    public int CurrentHealth { get; set; }
    public int CurrentMovementPoints { get; private set; }
    public bool HasAttacked { get; set; }
    public bool IsMoving { get; set; }
    public bool hasTurn { get; set; }
    public bool IsDead { get; private set; }
    public int Player { get; set; }
    public Tile CurrentTile { get; set; }

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetLayer(1);
    }

    public void SetUnitData(UnitData _unitData)
    {
        UnitData = _unitData;
        CurrentDamage = UnitData.baseDamage;
        CurrentHealth = UnitData.maxHealth;
        CurrentMovementPoints = UnitData.movementSpeed;
        GetComponent<SpriteRenderer>().sprite = UnitData.unitSprite;
    }
   
    public IEnumerator Move(Tile destination, Unit otherUnit = null)
    {
        IsMoving = true;
        Tile oldTile = CurrentTile;
        CurrentTile = destination;
        destination.currentUnit = this;
        oldTile.currentUnit = null;

        CurrentMovementPoints -= (int)GameManager.DistanceWithLines(transform.position, destination.Position);
        Selector.instance.MovingUnit = true;

        Vector2 finalPosition;

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

        if (destination.TileData.bonusUnit && destination.TileData.bonusUnit.unitName == destination.currentUnit.UnitData.unitName)
            CurrentDamage = UnitData.baseDamage + destination.TileData.bonusDamage;
        else
            CurrentDamage = UnitData.baseDamage;

        Selector.instance.SetSelectedInfo();
        Selector.instance.MovingUnit = false;
        IsMoving = false;

        if (otherUnit)
        {
            otherUnit.Hit(CurrentDamage);
            HasAttacked = true;
            Used();
        }

        hasTurn = false;
    }

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

    public void SetLayer(int layer)
    {
        spriteRenderer.sortingOrder = layer;
    }

    public void ResetUnit()
    {
        HasAttacked = false;
        CurrentMovementPoints = UnitData.movementSpeed;
        spriteRenderer.color = Color.white;
    }

    public void Used()
    {
        spriteRenderer.color = Color.red;
    }

}
