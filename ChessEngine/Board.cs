using ChessEngine.Enums;
using ChessEngine.Pieces;

namespace ChessEngine;

public class Board
{
    private readonly Piece?[,] _squares = new Piece?[8, 8];
    private PieceColor _lastMovedColor = PieceColor.Black;
    private int _materialBalance = 0;
    private HashSet<Position> _whiteAttackMap = new();
    private HashSet<Position> _blackAttackMap = new();

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
    
    public class GameStateDto
    {
        public string[][] Board { get; set; }
        public bool WhiteInCheck { get; set; }
        public bool BlackInCheck { get; set; }
    }
    
    public GameStateDto ToDto()
    {
        var boardDto = new string[8][];
        for (var i = 0; i < 8; i++)
        {
            boardDto[i] = new string[8];
            for (var j = 0; j < 8; j++)
            {
                var piece = _squares[i, j];
                if (piece == null) { boardDto[i][j] = ""; continue; }
                var colorPrefix = piece.Color == PieceColor.White ? "w" : "b";
                var typeChar = piece.Type == PieceType.Knight ? 'N' : piece.Type.ToString()[0];
                boardDto[i][j] = $"{colorPrefix}{typeChar}";
            }
        }

        return new GameStateDto
        {
            Board = boardDto,
            WhiteInCheck = IsKingChecked(PieceColor.White),
            BlackInCheck = IsKingChecked(PieceColor.Black)
        };
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
    
        if (piece == null || piece.Color == _lastMovedColor) return false;
        if (target != null && target.Color == piece.Color) return false;
        if (!piece.IsValidMove(from, to, this)) return false;
        
        var originalTarget = _squares[to.Row, to.Col];
        _squares[to.Row, to.Col] = piece;
        _squares[from.Row, from.Col] = null;
        var originalPos = piece.CurrentPosition;
        piece.CurrentPosition = to;
        
        UpdateAttackMaps();

        if (IsKingChecked(piece.Color))
        {
            _squares[from.Row, from.Col] = piece;
            _squares[to.Row, to.Col] = originalTarget;
            piece.CurrentPosition = originalPos;
            UpdateAttackMaps();
            Console.WriteLine("Ruch nielegalny - król byłby szachowany!");
            return false;
        }
        
        if (target != null)
        {
            var multiplier = (piece.Color == PieceColor.White) ? 1 : -1;
            _materialBalance += (target.Value * multiplier);
        }

        piece.HasMoved = true;
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

    public bool IsKingChecked(PieceColor kingColor)
    {
        Position kingPos = FindKing(kingColor);
        PieceColor enemyColor = kingColor == PieceColor.White ? PieceColor.Black : PieceColor.White;
    
        return IsFieldUnderAttack(kingPos, enemyColor);
    }
    
    private void UpdateAttackMaps()
    {
        _whiteAttackMap.Clear();
        _blackAttackMap.Clear();

        for (var r = 0; r < 8; r++) {
            for (var c = 0; c < 8; c++) {
                var piece = _squares[r, c];
                if (piece == null) continue;

                var attacked = piece.GetAttackedFields(this);
                if (piece.Color == PieceColor.White)
                    foreach (var pos in attacked) _whiteAttackMap.Add(pos);
                else
                    foreach (var pos in attacked) _blackAttackMap.Add(pos);
            }
        }
    }

    public bool IsFieldUnderAttack(Position pos, PieceColor attackerColor)
    {
        return attackerColor == PieceColor.White ? _whiteAttackMap.Contains(pos) : _blackAttackMap.Contains(pos);
    }
    
    public Position FindKing(PieceColor color)
    {
        for (var r = 0; r < 8; r++)
        {
            for (var c = 0; c < 8; c++)
            {
                var piece = _squares[r, c];
                if (piece != null && piece.Type == PieceType.King && piece.Color == color)
                {
                    return new Position(r, c);
                }
            }
        }
        
        throw new Exception($"Król koloru {color} zniknął z planszy!");
    }
}