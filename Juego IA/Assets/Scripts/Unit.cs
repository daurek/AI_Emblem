using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitData UnitData { get; private set; }
    public int CurrentDamage { get; private set; }
    public int CurrentHealth { get; private set; }
    public int CurrentMovementPoints { get; private set; }
    private bool hasAttacked;
    private bool hasMoved;
    public bool IsDead { get; private set; }

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
   
    public IEnumerator Move(Tile destination)
    {
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
        Selector.instance.MovingUnit = false;

    }

    public void Hit(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            IsDead = true;
            Destroy(gameObject);
            Selector.instance.SetHoverInfo();
        }

    }

    public void SetLayer(int layer)
    {
        spriteRenderer.sortingOrder = layer;
    }

}
