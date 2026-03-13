using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public class Queen : Piece
{
    public Queen(PieceColor color) : base(color, PieceType.Queen) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        var isDiagonal = Math.Abs(from.Row - to.Row) == Math.Abs(from.Col - to.Col);
        var isStraight = from.Row == to.Row || from.Col == to.Col;
        
        return isDiagonal || isStraight;
    }
}