using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public class Queen : Piece
{
    public Queen(PieceColor color, Position position) : base(color, PieceType.Queen, position) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        var isDiagonal = Math.Abs(from.Row - to.Row) == Math.Abs(from.Col - to.Col);
        var isStraight = from.Row == to.Row || from.Col == to.Col;
        
        if (!isDiagonal && !isStraight) return false;
        
        return board.IsPathClear(from, to);
    }
}