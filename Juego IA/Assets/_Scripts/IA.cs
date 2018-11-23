using UnityEngine;

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

    public void Play()
    {
        unitIndex = 1;
        PlayUnit(GameManager.instance.Player[1][0]);
    }

    public void PlayUnit(Unit unitToPlay)
    {   
        unitToPlay.hasTurn = true;
        ScoringMove bestMove = Negamax(CheckBoard(), unitToPlay, 1);
        StartCoroutine(unitToPlay.Move(bestMove.tile, bestMove.unit));
    }

    ScoringMove Negamax(Board _board, Unit _currentUnit, int _depth)
    {
        ScoringMove scoringMove = null;
        Unit bestUnitToAttack = null;
        Tile bestTile = null;
        int bestScore = 0;
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

        //return scoringMove;


        //if (_depth == MAX_DEPTH)
        //{
        //    //En los niveles impares el valor de la evaluación se invierte, para ajustarlo al comportamiento de negamax

        //    if (_depth % 2 == 1)
        //    {
        //        //scoringMove = new ScoringMove(_board.Evaluate(_board.activePlayer), bestTile, bestUnitToAttack );
        //        scoringMove = Negamax(_board.GenerateNewBoardFromMove(bestTile, _currentUnit, bestUnitToAttack), _currentUnit, _depth);
        //    }
        //    else
        //    {
        //        //scoringMove = new ScoringMove(-_board.Evaluate(_board.activePlayer), bestTile, bestUnitToAttack);
        //        scoringMove = Negamax(_board.GenerateNewBoardFromMove(bestTile, _currentUnit, bestUnitToAttack), _currentUnit, _depth);
        //        scoringMove.score *= -1;
        //    }

        //}
        //else
        if (_depth <= MAX_DEPTH)
        {
            bestScore = MINUS_INFINITE;

            Tile[] possibleMoves = _board.PossibleMoves(_currentUnit);

            foreach (Tile tileToMove in possibleMoves)
            {

                int score = 0;

                if (tileToMove.TileData.bonusUnit == _currentUnit.UnitData)
                {

                    score += 20;
                }
                else
                {
                    score += 2;
                }

                Tile[] possibleAttacks = _board.PossibleAttacks(_currentUnit, tileToMove);

                if (score > bestScore)
                {
                    bestScore = score;
                    bestTile = tileToMove;
                    bestUnitToAttack = null;
                }

                //if (possibleAttacks.Length == 0)
                //{
                //    scoringMove = Negamax(_board.GenerateNewBoardFromMove(tileToMove, _currentUnit, bestUnitToAttack), _currentUnit, _depth + 1);
                //    // Invertimos el signo para que, en cada nivel de profundidad, haga efecto el “nega” de Negamax
                //    bestScore -= scoringMove.score;
                //}


                foreach (Tile tileToAttack in possibleAttacks)
                {
                    // Able to kill
                    if (tileToAttack.currentUnit.CurrentHealth <= _currentUnit.CurrentDamage)
                    {
                        score += 20;
                    }
                    else
                    {
                        score += 10;
                    }

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestUnitToAttack = tileToAttack.currentUnit;
                        bestTile = tileToMove;
                    }



                    //// Recursividad
                    //scoringMove = Negamax(_board.GenerateNewBoardFromMove(tileToMove, _currentUnit, bestUnitToAttack), _currentUnit, _depth + 1);
                    //// Invertimos el signo para que, en cada nivel de profundidad, haga efecto el “nega” de Negamax
                    //bestScore -= scoringMove.score;
                    

                }





                // Actualizar mejor score si obtenemos una jugada mejor.

            }

            scoringMove = new ScoringMove(bestScore, bestTile, bestUnitToAttack);
            //_depth++;
        }
        
        return scoringMove;
    }

    private Board CheckBoard()
    {
        return new Board(GameManager.instance.tileMap);
    }
}
