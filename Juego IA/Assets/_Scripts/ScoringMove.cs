/// <summary>
/// Data Class that stores the score of a move, the destination tile and the unit it has to attack
/// </summary>
public class ScoringMove
{
    public int score;
    public Tile tile;
    public Unit unit;

    public ScoringMove(int _score, Tile _tile, Unit _unit)
    {
        score = _score;
        tile = _tile;
        unit = _unit;
    }
}