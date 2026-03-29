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
    
    public override List<Position> GetAttackedFields(Board board)
    {
        var fields = new List<Position>();
        int[] rowDirs = { -1, 1, 0, 0, -1, -1, 1, 1 };
        int[] colDirs = { 0, 0, -1, 1, -1, 1, -1, 1 };

        for (int i = 0; i < 8; i++)
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