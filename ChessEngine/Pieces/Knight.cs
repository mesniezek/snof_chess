using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public class Knight : Piece
{
    public Knight(PieceColor color, Position position) : base(color, PieceType.Knight, position) { }

    public override bool IsValidMove(Position from, Position to, Board board)
    {
        var rowDiff = Math.Abs(from.Row - to.Row);
        var colDiff = Math.Abs(from.Col - to.Col);
        
        return (rowDiff == 2 && colDiff == 1) || (rowDiff == 1 && colDiff == 2);
    }
    
    public override List<Position> GetAttackedFields(Board board)
    {
        var fields = new List<Position>();
        int[] rowOffsets = { -2, -2, -1, -1, 1, 1, 2, 2 };
        int[] colOffsets = { -1, 1, -2, 2, -2, 2, -1, 1 };

        for (var i = 0; i < 8; i++)
        {
            var targetRow = CurrentPosition.Row + rowOffsets[i];
            var targetCol = CurrentPosition.Col + colOffsets[i];

            if (targetRow >= 0 && targetRow < 8 && targetCol >= 0 && targetCol < 8)
            {
                fields.Add(new Position(targetRow, targetCol));
            }
        }
        return fields;
    }
}