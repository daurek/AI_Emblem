using UnityEngine;

public class IA : MonoBehaviour
{
    private Board board;
    public byte MAX_DEPTH = 2;

    public void Play()
    {
        ScoringMove move;
        ObservarTablero();
        //move = Negamax(board, 0);
        //Debug.Log("Jugada: " + move.movimiento + "// Score: " + move.score);
        //Mover(move.movimiento);
        GameManager.instance.ChangeTurn();
    }

    //ScoringMove Negamax(Board board, int depth)
    //{
    //    //byte bestMove = 0;
    //    int bestScore = 0;
    //    int currentScore;
    //    ScoringMove scoringMove;
    //    Board newBoard = null;

    //    int activePlayer = 0;
    //    Tile bestTile = null;
    //    Tile tile = null;
    //    int MINUS_INFINITE = 0;

    //    // Comprobar si hemos terminado de hacer recursión, por 2 posibles motivos:
    //    // 1. hemos llegado a una jugada terminal.
    //    // 2. hemos alcanzado la máxima profundidad que nos permite nuestra inteligencia.
    //    if (depth == MAX_DEPTH || board.IsEndOfGame())
    //    {
    //        // En los niveles impares el valor de la evaluación se invierte, para ajustarlo al comportamiento de negamax

    //        if (depth % 2 == 0)
    //        {                
    //            scoringMove = new ScoringMove(board.Evaluate(activePlayer), tile);
    //        }
    //        else
    //        {
    //            scoringMove = new ScoringMove(-board.Evaluate(activePlayer), tile);
    //        }
    //    }

    //    else
    //    {            
    //        bestScore = MINUS_INFINITE;
    //        byte[] possibleMoves;
    //        possibleMoves = board.PossibleMoves();
    //        foreach (byte move in possibleMoves)
    //        {
    //            newBoard = board.GenerateNewBoardFromMove(move);
    //            // Recursividad
    //            scoringMove = Negamax(newBoard, depth + 1);
    //            // Invertimos el signo para que, en cada nivel de profundidad, haga efecto el “nega” de Negamax
    //            currentScore = -scoringMove.score;
    //            // Actualizar mejor score si obtenemos una jugada mejor.
    //            if (currentScore > bestScore)
    //            {
    //                bestScore = currentScore;
    //                bestMove = move;
    //            }
    //        }
    //        scoringMove = new ScoringMove(bestScore, bestTile);
    //    }

    //    return scoringMove;
    //}

    private void ObservarTablero()
    {
        board = new Board(GameManager.instance.tileMap);
    }
}
