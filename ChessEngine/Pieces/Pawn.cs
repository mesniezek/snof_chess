using ChessEngine.Enums;
using ChessEngine;

namespace ChessEngine.Pieces;

public class Pawn : Piece
{
    public Pawn(PieceColor color, Position position) : base(color, PieceType.Pawn, position) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        var direction = Color == PieceColor.White ? -1 : 1;
        var startRow = Color == PieceColor.White ? 6 : 1;
        
        if (from.Col == to.Col && to.Row == from.Row + direction && board.GetPiece(to) == null)
            return true;
        
        if (!HasMoved && from.Col == to.Col && to.Row == from.Row + 2 * direction && board.GetPiece(to) == null)
            return true;
        
        return Math.Abs(from.Col - to.Col) == 1 && to.Row == from.Row + direction && board.GetPiece(to) != null;
    }
}