using Microsoft.AspNetCore.Mvc;
using ChessEngine;

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
}