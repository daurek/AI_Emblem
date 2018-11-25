using System;
using System.Collections.Generic;

/// <summary>
/// Data class that hold information about the tiles
/// </summary>
public class Board
{
    public List<List<Tile>> tileMap;
    public int activePlayer;

    public Board (List<List<Tile>> _tileMap)
    {        
        tileMap = _tileMap;
        activePlayer = GameManager.instance.PlayerTurn;
    }

    /// <summary>
    /// Returns the best possible attack with the given tile and unit
    /// </summary>
    /// <param name="tileToMove"></param>
    /// <param name="_currentUnit"></param>
    /// <returns></returns>
    public ScoringMove Evaluate(Tile tileToMove, Unit _currentUnit)
    {
        int score = 0;
        int bestAttackScore = 0;
        int attackScore = 0;
        Unit attackUnit = null;

        // If our unit is on a bonus tile then add more score
        if (tileToMove.TileData.bonusUnit == _currentUnit.UnitData)
        {
            score += 20;
        }
        // Otherwise the add a smaller score
        else
        {
            score += 2;
        }

        // Get an array of tiles of the possible attack that our unit can do around that tile
        Tile[] possibleAttacks = PossibleAttacks(_currentUnit, tileToMove);

        // Loop through every possible attack
        foreach (Tile tileToAttack in possibleAttacks)
        {   
            // Able to kill
            if (tileToAttack.currentUnit.CurrentHealth <= _currentUnit.CurrentDamage)
            {
                attackScore += 20;
            }
            // Only damage it
            else
            {
                attackScore += 10;
            }

            // If we get a new best attack then save it's data
            if (attackScore > bestAttackScore)
            {
                bestAttackScore = attackScore;
                attackUnit = tileToAttack.currentUnit;
            }

        }
        // Return the best attack that the unit can do
        return new ScoringMove(score + bestAttackScore, tileToMove, attackUnit);
    }

    /// <summary>
    /// Return an array of the possible moves around a tile
    /// </summary>
    /// <param name="_currentUnit"></param>
    /// <returns></returns>
    public Tile[] PossibleMoves(Unit _currentUnit) // Devuelve el número de casillas a las que puede ir la unidad
    {
        return GameManager.instance.UnitRangeIndicatorAI(_currentUnit.CurrentTile).ToArray();
    }

    /// <summary>
    /// Returns an array of the possible attacks around a tile
    /// </summary>
    /// <param name="_currentUnit"></param>
    /// <param name="_currentTile"></param>
    /// <returns></returns>
    public Tile[] PossibleAttacks(Unit _currentUnit, Tile _currentTile)
    {
        return GameManager.instance.UnitRangeIndicatorAIAttack(_currentTile, _currentUnit).ToArray();
    }

    /// <summary>
    /// Generates a new board after a movement
    /// </summary>
    /// <param name="_tileToMove"></param>
    /// <param name="_currentUnit"></param>
    /// <param name="_unitToAttack"></param>
    /// <returns></returns>
    public Board GenerateNewBoardFromMove(Tile _tileToMove, Unit _currentUnit, Unit _unitToAttack)
    {
        //Board newBoard = new Board(tileMap);

        _currentUnit.CurrentTile.currentUnit = null;
        _tileToMove.currentUnit = _currentUnit;
        _currentUnit.CurrentTile = _tileToMove;

        if (_unitToAttack)
        {
            _unitToAttack.CurrentHealth -= _currentUnit.CurrentDamage;
        }

        return this;
    }
}
