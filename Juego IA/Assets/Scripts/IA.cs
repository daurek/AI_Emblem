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
        ScoringMove move;

        Unit currentUnit = null;

        for (int i = 0; i < GameManager.instance.Player[1].Count; i++)
        {
            currentUnit = GameManager.instance.Player[1][i];
            move = Negamax(ObservarTablero(), currentUnit, 1);
            StartCoroutine(currentUnit.Move(move.tile));

        }
        
        //Debug.Log("Jugada: " + move.movimiento + "// Score: " + move.score);
        //Mover(move.movimiento);
     
        GameManager.instance.ChangeTurn();
    }

    ScoringMove Negamax(Board board, Unit currentUnit, int depth)
    {
        //byte bestMove = 0;
        int bestScore = 0;
        int currentScore = 0;
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
        if (depth == MAX_DEPTH)
        {
            bestScore = MINUS_INFINITE;
            Tile[] possibleMoves = board.PossibleMoves(currentUnit);

            foreach (Tile tileToMove in possibleMoves)
            {
                int tileScore = 0;

                if (tileToMove.TileData.bonusUnit == currentUnit.UnitData)
                {

                    tileScore += 20;
                }
                else
                {
                    tileScore += 2;
                }

                Tile[] possibleAttacks  = board.PossibleAttacks(currentUnit, tileToMove);
                print(possibleAttacks.Length);
                foreach (Tile tileToAttack in possibleAttacks)
                {
                    tileScore += 20;


                    //if (tileScore > bestScore)
                    //{
                    //    bestScore = tileScore;
                    //    bestTile = tileToMove;
                    //}

                }
                if (tileScore > bestScore)
                {
                    bestScore = tileScore;
                    bestTile = tileToMove;
                }

                //newBoard = board.GenerateNewBoardFromMove(tileToMove);
                // Recursividad
                //scoringMove = Negamax(newBoard, depth + 1);
                // Invertimos el signo para que, en cada nivel de profundidad, haga efecto el “nega” de Negamax
                //currentScore = -scoringMove.score;
                //// Actualizar mejor score si obtenemos una jugada mejor.

            }
            
            scoringMove = new ScoringMove(bestScore, bestTile);
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
