namespace Minesweeper
{
    public class Tile
    {
        public TileState TileState { get; set; } = TileState.Covered;
        public TileType TileType { get; set; } = TileType.Blank;

        public Tile()
        {

        }

        public Tile(TileState tileState, TileType tileType)
        {
            TileState = tileState;
            TileType = tileType;
        }
    }
    public enum TileState
    {
        Covered = 0,
        Flagged = 1,
        Uncovered = 2
    }

    public enum TileType
    {
        Blank = 0,
        Adjacent1 = 1,
        Adjacent2 = 2,
        Adjacent3 = 3,
        Adjacent4 = 4,
        Adjacent5 = 5,
        Adjacent6 = 6,
        Adjacent7 = 7,
        Adjacent8 = 8,
        Mine = 9,
        Explosion = 10
    }
}
