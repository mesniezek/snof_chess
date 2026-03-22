using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public class Rook : Piece
{
    public Rook(PieceColor color, Position position) : base(color, PieceType.Rook, position) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        if (from.Row != to.Row && from.Col != to.Col) return false;
        
        return board.IsPathClear(from, to);
    }
}