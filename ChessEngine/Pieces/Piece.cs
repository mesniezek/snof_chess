using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public abstract class Piece
{
    public PieceColor Color { get; }
    public PieceType Type { get; }
    public bool HasMoved { get; set; } = false;
    public Position CurrentPosition { get; set; }
    public int Value => Type switch
    {
        PieceType.Pawn => 1,
        PieceType.Knight => 3,
        PieceType.Bishop => 3,
        PieceType.Rook => 5,
        PieceType.Queen => 9,
        PieceType.King => int.MaxValue,
        _ => throw new ArgumentOutOfRangeException()
    };
    
    public virtual List<Position> GetAttackedFields(Board board) 
    {
        return new List<Position>(); 
    }

    protected Piece(PieceColor color, PieceType type, Position position)
    {
        Color = color;
        Type = type;
        CurrentPosition = position;
    }
    
    public abstract bool IsValidMove(Position from, Position to, Board board);

    public IList<Position> GetLegalMoves(Board board)
    {
        var legalMoves = new List<Position>();
        for (var row = 0; row < 8; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                var to = new Position(row, col);
                if (!IsValidMove(CurrentPosition, to, board)) continue;
            
                var target = board.GetPiece(to);
                if (target != null && target.Color == this.Color) continue;

                var originalPos = this.CurrentPosition;
            
                if (board.SimulateAndCheckIfSafe(originalPos, to, this.Color))
                {
                    legalMoves.Add(to);
                }
            }
        }
        return legalMoves;
    }
}
