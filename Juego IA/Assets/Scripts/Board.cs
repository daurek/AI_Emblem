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
        throw new NotImplementedException();
    }

    public byte[] PossibleMoves()
    {
        throw new NotImplementedException();
    }

    public Board GenerateNewBoardFromMove(byte move)
    {
        throw new NotImplementedException();
    }
}

