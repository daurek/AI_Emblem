using System;
using System.Collections.Generic;

public class Board
{
    public List<List<Tile>> tileMap;
    public int activePlayer;

    public Board (List<List<Tile>> _tileMap)
    {        
        tileMap = _tileMap;
        activePlayer = GameManager.instance.PlayerTurn;
    }

    public bool IsEndOfGame()
    {
        throw new NotImplementedException();
    }

    public int Evaluate(int _activePlayer)
    {   



        //int NumberOfTilesIsPosibleToMove = 1;
        //int NumberOfEnemiesIsPosibleToAtack = 3;
        //int score = 0;

        ////if (EsVictoria(jugador))
        ////{
        ////    return 100;
        ////}
        ////else if (EsVictoria(Contrario(jugador)))
        ////{
        ////    return -100;
        ////}
        ////else if (EsTableroLleno()) return 1;

        
        //for (int i = 0; i < NumberOfTilesIsPosibleToMove; i++)
        //{
        //    if (i == NumberOfTilesIsPosibleToMove - 1)
        //    {
        //        score += 20;
        //        //Debug.Log("score1: " + score);
        //    }

        //    for (int j = 0; j < NumberOfEnemiesIsPosibleToAtack; j++)
        //    {
        //        if (j == NumberOfEnemiesIsPosibleToAtack - 1)
        //        {
        //            score += 20;
        //            //Debug.Log("score2: " + score);
        //        }                
        //    }
        //}
        //// Pediente programar evaluaciones intermedias.
        ////Debug.Log("score: " + score);
        return 2;
    }

    public Tile[] PossibleMoves(Unit _currentUnit) // Devuelve el número de casillas a las que puede ir la unidad
    {
        return GameManager.instance.UnitRangeIndicatorAI(_currentUnit.CurrentTile).ToArray();
    }

    public Tile[] PossibleAttacks(Unit _currentUnit, Tile _currentTile)
    {
        return GameManager.instance.UnitRangeIndicatorAIAttack(_currentTile, _currentUnit).ToArray();
    }

    public Board GenerateNewBoardFromMove(Tile _tileToMove, Unit _currentUnit, Unit _unitToAttack) // Genera un nuevo tablero a partir del nuevo movimiento
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



















//using System.Collections;
//using System.Collections.Generic;

//public class Tablero
//{
//    public byte[] espacios; /*  0 1 2
//                                3 4 5
//                                6 7 8*/
//                            // 0 Vacío
//                            // 1 Jugador1
//                            // 2 Jugador2
//    public byte[,] espacios4;
//    // 0 Vacío
//    // 1 Jugador1
//    // 2 Jugador2

//    private byte columnas = 7;
//    private byte filas = 6;

//    public byte jugadorActivo;

//    public Tablero()
//    {
//        espacios4 = new byte[7, 6];
//    }

//    // Comprobar en el tablero si estamos en una situación terminal (victoria o empate).
//    public bool EsFinalJuego()
//    {
//        if (EsVictoria(1)) return true;
//        else if (EsVictoria(2)) return true;
//        else if (EsTableroLleno()) return true;
//        return false;
//    }
//    private bool EsVictoria(byte jugador)
//    {
//        if (VictoriaHorizontal(jugador) ||
//            VictoriaVertical(jugador) ||
//            VictoriaDiagonalDescendente(jugador) ||
//            VictoriaDiagonalAscendente(jugador)
//           )
//            return true;

//        return false;
//    }

//    private bool VictoriaHorizontal(byte jugador)
//    {
//        for (byte fila = 0; fila < filas; ++fila)
//        {
//            for (byte columna = 0; columna < columnas - 3; ++columna)
//            {
//                if ((espacios4[columna, fila] == jugador) &&
//                    (espacios4[columna + 1, fila] == jugador) &&
//                    (espacios4[columna + 2, fila] == jugador) &&
//                    (espacios4[columna + 3, fila] == jugador))
//                {
//                    GameController.instance.Print("VH" + columna + "/" + fila);
//                    return true;
//                }
//            }
//        }
//        return false;
//    }
//    private bool VictoriaVertical(byte jugador)
//    {
//        for (byte fila = 0; fila < filas - 3; ++fila)
//        {
//            for (byte columna = 0; columna < columnas; ++columna)
//            {
//                if ((espacios4[columna, fila] == jugador) &&
//                    (espacios4[columna, fila + 1] == jugador) &&
//                    (espacios4[columna, fila + 2] == jugador) &&
//                    (espacios4[columna, fila + 3] == jugador))
//                {
//                    GameController.instance.Print("VV" + columna + "/" + fila);
//                    return true;
//                }
//            }
//        }
//        return false;
//    }
//    private bool VictoriaDiagonalDescendente(byte jugador)
//    {
//        for (byte fila = 0; fila < filas - 3; ++fila)
//        {
//            for (byte columna = 0; columna < columnas - 3; ++columna)
//            {
//                if ((espacios4[columna, fila] == jugador) &&
//                    (espacios4[columna + 1, fila + 1] == jugador) &&
//                    (espacios4[columna + 2, fila + 2] == jugador) &&
//                    (espacios4[columna + 3, fila + 3] == jugador))
//                {
//                    GameController.instance.Print("VD" + columna + "/" + fila);
//                    return true;
//                }
//            }
//        }
//        return false;
//    }
//    private bool VictoriaDiagonalAscendente(byte jugador)
//    {
//        for (byte fila = 3; fila < filas; ++fila)
//        {
//            for (byte columna = 0; columna < columnas - 3; ++columna)
//            {
//                if ((espacios4[columna, fila] == jugador) &&
//                    (espacios4[columna + 1, fila - 1] == jugador) &&
//                    (espacios4[columna + 2, fila - 2] == jugador) &&
//                    (espacios4[columna + 3, fila - 3] == jugador))
//                {
//                    GameController.instance.Print("VA" + columna + "/" + fila);
//                    return true;
//                }
//            }
//        }
//        return false;
//    }

//    private bool EsTableroLleno()
//    {
//        for (byte columna = 0; columna < columnas; ++columna)
//        {
//            if (espacios4[columna, 0] == 0)
//            {
//                return false;
//            }
//        }
//        return true;
//    }

//    // Función de evaluación estática
//    public short Evaluar(byte jugador)
//    {
//        if (EsVictoria(jugador))
//        {
//            return 100;
//        }
//        else if (EsVictoria(Contrario(jugador)))
//        {
//            return -100;
//        }
//        else if (EsTableroLleno()) return 1;

//        // Pediente programar evaluaciones intermedias.
//        return 0;
//    }

//    private byte Contrario(byte jugador)
//    {
//        if (jugador == 1) return 2;
//        else return 1;
//    }

//    // Chequea el array de posiciones, y devuelve las posiciones vacías.
//    public byte[] MovimientosPosibles()
//    {
//        byte[] movimientos = null;
//        byte cuenta = 0;
//        for (byte columna = 0; columna < columnas; ++columna)
//        {
//            if (espacios4[columna, 0] == 0)
//            {
//                cuenta++;
//            }
//        }

//        movimientos = new byte[cuenta];
//        byte indice = 0;
//        for (byte columna = 0; columna < columnas; ++columna)
//        {
//            if (espacios4[columna, 0] == 0)
//            {
//                movimientos[indice] = columna;
//                indice++;
//            }
//        }

//        return movimientos;
//    }

//    // Crear un objeto Tablero nuevo a partir de aplicar el movimiento que le indican.
//    public Tablero GenerarTablero(byte move)
//    {
//        Tablero newTablero = new Tablero();
//        for (byte fila = 0; fila < filas; ++fila)
//        {
//            for (byte columna = 0; columna < columnas; ++columna)
//            {
//                newTablero.espacios4[columna, fila] = espacios4[columna, fila];
//            }
//        }

//        for (byte fila = (byte)(filas - 1); fila >= 0; --fila)
//        {
//            if (espacios4[move, fila] == 0)
//            {
//                espacios4[move, fila] = jugadorActivo;
//                break;
//            }
//        }

//        newTablero.jugadorActivo = Contrario(jugadorActivo);

//        return newTablero;
//    }

//}