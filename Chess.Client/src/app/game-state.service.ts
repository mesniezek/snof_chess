import { Injectable, signal } from '@angular/core';
import { ChessService } from './chess.service';

@Injectable({ providedIn: 'root' })
export class GameStateService {
  board = signal<string[][]>([]);
  whiteInCheck = signal(false);
  blackInCheck = signal(false);
  whiteAdvantage = signal(0);
  blackAdvantage = signal(0);
  isGameOver = signal(false);
  winner = signal<string | null>(null);
  selectedSquare = signal<{ row: number, col: number } | null>(null);
  legalMoves = signal<{ row: number, col: number }[]>([]);
  whiteCapturedPieces = signal<string[]>([]);
  blackCapturedPieces = signal<string[]>([]);

  constructor(private chessService: ChessService) {}

  updateFromDto(data: any) {
    this.board.set(data.board);
    this.whiteInCheck.set(data.whiteInCheck);
    this.blackInCheck.set(data.blackInCheck);
    this.isGameOver.set(data.isGameOver || false);
    this.winner.set(data.winner || null);
    this.selectedSquare.set(null);
    this.legalMoves.set([]);
    this.whiteCapturedPieces.set(data.whiteCapturedPieces || []);
    this.blackCapturedPieces.set(data.blackCapturedPieces || []);
  }

  updateAdvantages() {
    ['white', 'black'].forEach(color => {
      this.chessService.getAdvantage(color as 'white' | 'black').subscribe((res: any) => {
        const val = res && typeof res === 'object' ? (res.value ?? res.advantage ?? 0) : (res ?? 0);
        color === 'white' ? this.whiteAdvantage.set(val) : this.blackAdvantage.set(val);
      });
    });
  }

  reset(data: any) {
    this.updateFromDto(data);
    this.whiteAdvantage.set(0);
    this.blackAdvantage.set(0);
  }
}
