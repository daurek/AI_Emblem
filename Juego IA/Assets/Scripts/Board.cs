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
        
}

