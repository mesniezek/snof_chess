using Microsoft.AspNetCore.Mvc;
using ChessEngine;
using ChessEngine.Enums;
using ChessEngine.Pieces;

namespace ChessApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChessController : ControllerBase
{
    private readonly Board _board;
    
    public ChessController(Board board)
    {
        _board = board;
    }

    [HttpGet("board")]
    public IActionResult GetBoard()
    {
        return Ok(_board.ToDto());
    }

    [HttpPost("move")]
    public IActionResult Move([FromBody] MoveRequest request)
    {
        var from = new Position(request.FromRow, request.FromCol);
        var to = new Position(request.ToRow, request.ToCol);
        var success = _board.HandleMove(from, to);

        if (success)
        {
            return Ok(_board.ToDto());
        }
        
        return BadRequest(new { message = "Legal move validation failed or path blocked!" });
    }

    [HttpGet("legal-moves")]
    public IActionResult GetLegalMoves([FromQuery] int row, [FromQuery] int col)
    {
        var pos = new Position(row, col);
        var piece = _board.GetPiece(pos);

        if (piece == null) 
            return NotFound(new { message = "No piece at this position" });

        var legalMoves = piece.GetLegalMoves(_board);
    
        return Ok(legalMoves.Select(p => new { p.Row, p.Col }));
    }
    
    [HttpPost("reset")]
    public IActionResult ResetBoard()
    {
        _board.InitializeBoard(); 
        return Ok(_board.ToDto());
    }
    
    [HttpGet("advantage")]
    public IActionResult CheckAdvantage(PieceColor color)
    {
        var advantage = _board.GetAdvantage(color);
        return Ok(new { advantage });
    }
}