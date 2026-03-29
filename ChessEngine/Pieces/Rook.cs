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
    
    public override List<Position> GetAttackedFields(Board board)
    {
        var fields = new List<Position>();
        int[] rowDirs = { -1, 1, 0, 0 };
        int[] colDirs = { 0, 0, -1, 1 };

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