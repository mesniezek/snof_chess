using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public class King : Piece
{
    public King(PieceColor color) : base(color, PieceType.King) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        return Math.Abs(from.Row - to.Row) <= 1 && Math.Abs(from.Col - to.Col) <= 1;
    }
}