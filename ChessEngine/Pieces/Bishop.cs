using ChessEngine.Enums;
using ChessEngine;

namespace ChessEngine.Pieces;

public class Bishop : Piece
{
    public Bishop(PieceColor color, Position position) : base(color, PieceType.Bishop, position) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        if (Math.Abs(from.Row - to.Row) != Math.Abs(from.Col - to.Col)) return false;

        return board.IsPathClear(from, to);
    }
}