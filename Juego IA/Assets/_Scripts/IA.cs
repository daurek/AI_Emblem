using UnityEngine;

/// <summary>
/// Handles the IA movement, attack and management
/// </summary>
public class IA : MonoBehaviour
{
    public static IA instance;

    private byte MAX_DEPTH = 1;
    private int unitIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (GameManager.instance.PlayerTurn != 0)
        {
            if (unitIndex != GameManager.instance.Player[1].Count)
            {
                if (!GameManager.instance.Player[1][unitIndex - 1].hasTurn)
                {
                    PlayUnit(GameManager.instance.Player[1][unitIndex]);
                    unitIndex++;
                }
            }
            else
            {
                GameManager.instance.ChangeTurn();
            }
        }
    }

    /// <summary>
    /// Starts the IA movement
    /// </summary>
    public void Play()
    {
        unitIndex = 1;
        PlayUnit(GameManager.instance.Player[1][0]);
    }

    /// <summary>
    /// Plays the given unit after looking for it's best move
    /// </summary>
    /// <param name="unitToPlay"></param>
    public void PlayUnit(Unit unitToPlay)
    {   
        unitToPlay.hasTurn = true;
        ScoringMove bestMove = Negamax(CheckBoard(), unitToPlay, 1);
        StartCoroutine(unitToPlay.Move(bestMove.tile, bestMove.unit));
    }

    /// <summary>
    /// Runs through the given board and return the best move for the given unit
    /// </summary>
    /// <param name="_board"></param>
    /// <param name="_currentUnit"></param>
    /// <param name="_depth"></param>
    /// <returns></returns>
    ScoringMove Negamax(Board _board, Unit _currentUnit, int _depth)
    {
        ScoringMove scoringMove = new ScoringMove(0, null, null);
        ScoringMove bestScoringMove = new ScoringMove(0,null,null);

        if (_depth <= MAX_DEPTH)
        {
            Tile[] possibleMoves = _board.PossibleMoves(_currentUnit);

            // Loop through every possible tile that the unit can move
            foreach (Tile tileToMove in possibleMoves)
            {
                scoringMove = _board.Evaluate(tileToMove, _currentUnit);

                // Take the best move
                if (bestScoringMove.score < scoringMove.score)
                {
                    bestScoringMove = scoringMove;
                }
            }
        }
        
        return bestScoringMove;
    }

    /// <summary>
    /// Creates a new board
    /// </summary>
    /// <returns></returns>
    private Board CheckBoard()
    {
        return new Board(GameManager.instance.tileMap);
    }
}
