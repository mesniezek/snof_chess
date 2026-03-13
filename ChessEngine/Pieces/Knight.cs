using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public class Knight : Piece
{
    public Knight(PieceColor color) : base(color, PieceType.Knight) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        var rowDiff = Math.Abs(from.Row - to.Row);
        var colDiff = Math.Abs(from.Col - to.Col);
        
        return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
    }
}