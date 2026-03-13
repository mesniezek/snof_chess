using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public class Rook : Piece
{
    public Rook(PieceColor color) : base(color, PieceType.Rook) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        if (from.Row != to.Row && from.Col != to.Col) return false;
        
        return board.IsPathClear(from, to);
    }
}