using ChessEngine.Enums;
using ChessEngine.Pieces;

namespace ChessEngine;

public class Board
{
    private readonly Piece?[,] _squares = new Piece?[8, 8];

    public Board()
    {
        InitializeBoard();
    }

    private void InitializeBoard()
    {
        _squares[0, 0] = new Rook(PieceColor.Black);
        _squares[0, 1] = new Knight(PieceColor.Black);
        _squares[0, 2] = new Bishop(PieceColor.Black);
        _squares[0, 3] = new Queen(PieceColor.Black);
        _squares[0, 4] = new King(PieceColor.Black);
        _squares[0, 5] = new Bishop(PieceColor.Black);
        _squares[0, 6] = new Knight(PieceColor.Black);
        _squares[0, 7] = new Rook(PieceColor.Black);
        
        for (var col = 0; col < 8; col++)
        {
            _squares[1, col] = new Pawn(PieceColor.Black);
        }

        for (var col = 0; col < 8; col++)
        {
            _squares[6, col] = new Pawn(PieceColor.White);
        }

        _squares[7, 0] = new Rook(PieceColor.White);
        _squares[7, 1] = new Knight(PieceColor.White);
        _squares[7, 2] = new Bishop(PieceColor.White);
        _squares[7, 3] = new Queen(PieceColor.White);
        _squares[7, 4] = new King(PieceColor.White);
        _squares[7, 5] = new Bishop(PieceColor.White);
        _squares[7, 6] = new Knight(PieceColor.White);
        _squares[7, 7] = new Rook(PieceColor.White);
    }

    public Piece? GetPiece(Position pos) => _squares[pos.Row, pos.Col];

    public bool MovePiece(Position from, Position to)
    {
        var piece = GetPiece(from);
        if (piece == null || !piece.IsValidMove(from, to, this)) return false;

        _squares[to.Row, to.Col] = piece;
        _squares[from.Row, from.Col] = null;
        piece.HasMoved = true;
        return true;
    }
    
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
        
        if (target != null && target.Color == piece.Color) return false;
        
        if (!piece.IsValidMove(from, to, this)) return false;
        
        _squares[to.Row, to.Col] = piece;
        _squares[from.Row, from.Col] = null;
        piece.HasMoved = true;

        return true;
    }
}