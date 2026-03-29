import { Injectable, signal } from '@angular/core';
import { ChessService } from './chess.service';

@Injectable({ providedIn: 'root' })
export class GameStateService {
  board = signal<string[][]>([]);
  whiteInCheck = signal(false);
  blackInCheck = signal(false);
  whiteAdvantage = signal(0);
  blackAdvantage = signal(0);

  selectedSquare = signal<{ row: number, col: number } | null>(null);
  legalMoves = signal<{ row: number, col: number }[]>([]);

  constructor(private chessService: ChessService) {}

  updateFromDto(data: any) {
    this.board.set(data.board);
    this.whiteInCheck.set(data.whiteInCheck);
    this.blackInCheck.set(data.blackInCheck);
    this.selectedSquare.set(null);
    this.legalMoves.set([]);
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
