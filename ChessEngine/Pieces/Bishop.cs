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
    
    public override List<Position> GetAttackedFields(Board board)
    {
        var fields = new List<Position>();
        int[] rowDirs = { -1, -1, 1, 1 };
        int[] colDirs = { -1, 1, -1, 1 };

        for (int i = 0; i < 4; i++)
        {
            int r = CurrentPosition.Row + rowDirs[i];
            int c = CurrentPosition.Col + colDirs[i];

            while (r >= 0 && r < 8 && c >= 0 && c < 8)
            {
                var target = new Position(r, c);
                fields.Add(target);

                if (board.GetPiece(target) != null) break;

                r += rowDirs[i];
                c += colDirs[i];
            }
        }
        return fields;
    }
}