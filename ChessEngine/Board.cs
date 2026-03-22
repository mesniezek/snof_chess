using ChessEngine.Enums;
using ChessEngine.Pieces;

namespace ChessEngine;

public class Board
{
    private readonly Piece?[,] _squares = new Piece?[8, 8];
    private PieceColor _lastMovedColor = PieceColor.Black;
    private int _materialBalance = 0;

    public Board()
    {
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        ClearBoard();
        _squares[0, 0] = new Rook(PieceColor.Black, new Position(0, 0));
        _squares[0, 1] = new Knight(PieceColor.Black, new Position(0, 1));
        _squares[0, 2] = new Bishop(PieceColor.Black, new Position(0, 2));
        _squares[0, 3] = new Queen(PieceColor.Black, new Position(0, 3));
        _squares[0, 4] = new King(PieceColor.Black, new Position(0, 4));
        _squares[0, 5] = new Bishop(PieceColor.Black, new Position(0, 5));
        _squares[0, 6] = new Knight(PieceColor.Black, new Position(0, 6));
        _squares[0, 7] = new Rook(PieceColor.Black, new Position(0, 7));
        
        for (var col = 0; col < 8; col++)
        {
            _squares[1, col] = new Pawn(PieceColor.Black, new Position(1, col));
        }

        for (var col = 0; col < 8; col++)
        {
            _squares[6, col] = new Pawn(PieceColor.White, new Position(6, col));
        }

        _squares[7, 0] = new Rook(PieceColor.White, new Position(7, 0));
        _squares[7, 1] = new Knight(PieceColor.White, new Position(7, 1));
        _squares[7, 2] = new Bishop(PieceColor.White, new Position(7, 2));
        _squares[7, 3] = new Queen(PieceColor.White, new Position(7, 3));
        _squares[7, 4] = new King(PieceColor.White, new Position(7, 4));
        _squares[7, 5] = new Bishop(PieceColor.White, new Position(7, 5));
        _squares[7, 6] = new Knight(PieceColor.White, new Position(7, 6));
        _squares[7, 7] = new Rook(PieceColor.White, new Position(7, 7));
    }

    public Piece? GetPiece(Position pos) => _squares[pos.Row, pos.Col];
    
    public string[][] ToDto()
    {
        var dto = new string[8][];
        for (var i = 0; i < 8; i++)
        {
            dto[i] = new string[8];
            for (var j = 0; j < 8; j++)
            {
                var piece = _squares[i, j];
                if (piece == null)
                {
                    dto[i][j] = "";
                    continue;
                }

                var colorPrefix = piece.Color == PieceColor.White ? "w" : "b";
                var typeChar = piece.Type == PieceType.Knight ? 'N' : piece.Type.ToString()[0];
            
                dto[i][j] = $"{colorPrefix}{typeChar}";
            }
        }
        return dto;
    }
    
    public bool IsPathClear(Position from, Position to)
    {
        var rowDiff = to.Row - from.Row;
        var colDiff = to.Col - from.Col;
        
        var rowStep = rowDiff == 0 ? 0 : rowDiff / Math.Abs(rowDiff);
        var colStep = colDiff == 0 ? 0 : colDiff / Math.Abs(colDiff);

        var currentRow = from.Row + rowStep;
        var currentCol = from.Col + colStep;
        
        while (currentRow != to.Row || currentCol != to.Col)
        {
            if (_squares[currentRow, currentCol] != null)
            {
                return false;
            }
            currentRow += rowStep;
            currentCol += colStep;
        }

        return true;
    }
    
    public bool HandleMove(Position from, Position to)
    {
        var piece = _squares[from.Row, from.Col];
        var target = _squares[to.Row, to.Col];
        
        if (piece == null) return false;
        
        if (piece.Color == _lastMovedColor) return false;
        
        if (target != null && target.Color == piece.Color) return false;

        if (!piece.IsValidMove(from, to, this)) return false;
        
        if (target != null)
        {
            int multiplier = (piece.Color == PieceColor.White) ? 1 : -1;
            _materialBalance += (target.Value * multiplier);
        }
        
        _squares[to.Row, to.Col] = piece;
        _squares[from.Row, from.Col] = null;
        
        piece.HasMoved = true;
        piece.CurrentPosition = to;
        
        _lastMovedColor = piece.Color;

        return true;
    }
    
    private void ClearBoard()
    {
        for (var row = 0; row < 8; row++)
        {
            for (var col = 0; col < 8; col++)
            {
                _squares[row, col] = null;
            }
        }
        _lastMovedColor = PieceColor.Black;
        _materialBalance = 0;
    }
    
    public int GetAdvantage(PieceColor color)
    {
        if (color == PieceColor.White)
            return _materialBalance > 0 ? _materialBalance : 0;
        
        return _materialBalance < 0 ? Math.Abs(_materialBalance) : 0;
    }
}