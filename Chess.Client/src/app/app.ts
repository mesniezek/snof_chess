import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChessService } from './chess.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  board: string[][] = [];

  constructor(private chessService: ChessService) {}

  ngOnInit() {
    this.chessService.getBoard().subscribe({
      next: (data) => {
        this.board = data;
        console.log('Szachownica załadowana:', data);
      },
      error: (err) => console.error('Błąd API:', err)
    });
  }

  selectedSquare: { row: number, col: number } | null = null;

  onSquareClick(row: number, col: number) {
    if (!this.selectedSquare) {
      if (this.board[row][col] !== "") {
        this.selectedSquare = { row, col };
      }
    }
    else {
      this.chessService.movePiece(
        this.selectedSquare.row,
        this.selectedSquare.col,
        row,
        col
      ).subscribe({
        next: (newBoard) => {
          this.board = newBoard;
          this.selectedSquare = null;
        },
        error: (err) => {
          console.error("Ruch niedozwolony!", err);
          this.selectedSquare = null;
        }
      });
    }
  }

  getPieceIcon(piece: string): string {
    const icons: { [key: string]: string } = {
      'wR': '♖', 'wN': '♘', 'wB': '♗', 'wQ': '♕', 'wK': '♔', 'wP': '♙',
      'bR': '♜', 'bN': '♞', 'bB': '♝', 'bQ': '♛', 'bK': '♚', 'bP': '♟'
    };
    return icons[piece] || '';
  }
}
