using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public class King : Piece
{
    public King(PieceColor color, Position position) : base(color, PieceType.King, position) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        return Math.Abs(from.Row - to.Row) <= 1 && Math.Abs(from.Col - to.Col) <= 1;
    }
    
    public override List<Position> GetAttackedFields(Board board)
    {
        var fields = new List<Position>();
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0) continue;

                int r = CurrentPosition.Row + dr;
                int c = CurrentPosition.Col + dc;

                if (r >= 0 && r < 8 && c >= 0 && c < 8)
                {
                    fields.Add(new Position(r, c));
                }
            }
        }
        return fields;
    }
}