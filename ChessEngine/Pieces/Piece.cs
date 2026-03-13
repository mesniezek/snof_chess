using ChessEngine.Enums;

namespace ChessEngine.Pieces;

public abstract class Piece
{
    public PieceColor Color { get; }
    public PieceType Type { get; }
    public bool HasMoved { get; set; } = false;

    protected Piece(PieceColor color, PieceType type)
    {
        Color = color;
        Type = type;
    }
    
    public abstract bool IsValidMove(Position from, Position to, Board board);
}
