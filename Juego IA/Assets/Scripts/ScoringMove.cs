public class ScoringMove
{
    public Tile tile;
    public Unit unit;
    // crear una unidad
    public int score;

    public ScoringMove(int _score, Tile _tile, Unit _unit)
    {
        tile = _tile;
        score = _score;
        unit = _unit;
    }
}


