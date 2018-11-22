using UnityEngine;

public class IA : MonoBehaviour
{

    public static IA instance;

    private Board board;
    private byte MAX_DEPTH = 1;

    private void Awake()
    {
        instance = this;
    }

    public void Play()
    {
        ScoringMove bestMove;

        Unit currentUnit = null;

        for (int i = 0; i < GameManager.instance.Player[1].Count; i++)
        {
            currentUnit = GameManager.instance.Player[1][i];
            bestMove = Negamax(ObservarTablero(), currentUnit, 1);
            StartCoroutine(currentUnit.Move(bestMove.tile));
            Debug.Log(bestMove.unit);
            if (bestMove.unit)
            {
                //while (currentUnit.IsMoving)
                //{

                //}

                bestMove.unit.Hit(currentUnit.CurrentDamage);
            }
            
        }
        
        //Debug.Log("Jugada: " + move.movimiento + "// Score: " + move.score);
        //Mover(move.movimiento);
     
        GameManager.instance.ChangeTurn();
    }

    ScoringMove Negamax(Board _board, Unit _currentUnit, int _depth)
    {
        //byte bestMove = 0;
        int bestAttackScore = 0;
        int totalScore = 0;
        int bestTotalScore = 0;
        Unit bestUnitToAttack = null;
        //int currentScore = 0;
        ScoringMove scoringMove = null;
       // Board newBoard = null;

        //int activePlayer = 0;
        Tile bestTile = null;
        //Tile tile = null;
        int MINUS_INFINITE = 0;

        // Comprobar si hemos terminado de hacer recursión, por 2 posibles motivos:
        // 1. hemos llegado a una jugada terminal.
        // 2. hemos alcanzado la máxima profundidad que nos permite nuestra inteligencia.
        //if (depth == MAX_DEPTH)  //|| board.IsEndOfGame())
        //{
        //    // En los niveles impares el valor de la evaluación se invierte, para ajustarlo al comportamiento de negamax

        //    if (depth % 2 == 0)
        //    {
        //        scoringMove = new ScoringMove(board.Evaluate(activePlayer), tile);

        //    }
        //    else
        //    {
        //        scoringMove = new ScoringMove(-board.Evaluate(activePlayer), tile);
        //    }
        //}

        //Debug.Log("scoringMove.score: " + scoringMove.score);
        //Debug.Log("scoringMove.tile: " + scoringMove.tile);
        //return scoringMove;
        if (_depth == MAX_DEPTH)
        {
            bestAttackScore = MINUS_INFINITE;
            Tile[] possibleMoves = _board.PossibleMoves(_currentUnit);
            //Unit bestUnitToAttack = null;

            foreach (Tile tileToMove in possibleMoves)
            {
                int tileScore = 0;

                if (tileToMove.TileData.bonusUnit == _currentUnit.UnitData)
                {

                    tileScore += 20;
                }
                else
                {
                    tileScore += 2;
                }

                Tile[] possibleAttacks = _board.PossibleAttacks(_currentUnit, tileToMove);
                bestAttackScore = 0;

                foreach (Tile tileToAttack in possibleAttacks)
                {
                    int attackScore = 0;

                    // Able to kill
                    if (tileToAttack.currentUnit.CurrentHealth <= _currentUnit.CurrentDamage) 
                    {
                        attackScore += 20;
                    }
                    else 
                    {

                        attackScore += 10;
                    }

                    if (attackScore > bestAttackScore)
                    {
                        bestAttackScore = attackScore;
                        bestUnitToAttack = tileToAttack.currentUnit;
                        Debug.Log(bestUnitToAttack);
                    }

                    // tileScore = 20 attackScore = 20 totalScore = 40

                    //if (attackScore + tileScore > bestScore)
                    //{
                    //    totalScore = attackScore + tileScore;
                    //    bestUnitToAttack = tileToAttack.currentUnit; 
                    //}

                    //if (attackScore > bestAttackScore)
                    //{
                    //    totalScore = attackScore + tileScore;
                    //    bestAttackScore = attackScore;
                    //    bestUnitToAttack = tileToAttack.currentUnit;
                    //}
                }

                totalScore = bestAttackScore + tileScore;

                if (totalScore > bestTotalScore)
                {
                    bestTotalScore = totalScore;
                    bestTile = tileToMove;

                    //if (bestAttackScore < tileScore)
                    //{

                    //}
                }



                //if (totalScore > bestAttackScore)
                //{
                //    bestAttackScore = totalScore;
                //    bestTile = tileToMove;
                //}

                //newBoard = board.GenerateNewBoardFromMove(tileToMove);
                // Recursividad
                //scoringMove = Negamax(newBoard, depth + 1);
                // Invertimos el signo para que, en cada nivel de profundidad, haga efecto el “nega” de Negamax
                //currentScore = -scoringMove.score;
                //// Actualizar mejor score si obtenemos una jugada mejor.

            }
            
            scoringMove = new ScoringMove(bestAttackScore, bestTile, bestUnitToAttack);
            //depth++;
            //Debug.Log("Score: " + scoringMove.score);
            //Debug.Log("Tile: " + scoringMove.tile);
            //Debug.Log("BonusUnit: " + scoringMove.tile.TileData.bonusUnit);
            //Debug.Log("Current Unit: " + currentUnit.UnitData);

        }
        
        return scoringMove;
    }

    private Board ObservarTablero()
    {
        return new Board(GameManager.instance.tileMap);
    }
}
