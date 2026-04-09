import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChessService } from './chess.service';
import { GameTimerService } from './game-timer.service';
import { GameStateService } from './game-state.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit, OnDestroy {
  badSquare: { row: number, col: number } | null = null;

  constructor(
    private chessService: ChessService,
    public timer: GameTimerService,
    public state: GameStateService,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.loadInitialData();
    this.timer.startClock();
  }

  ngOnDestroy() {
    this.timer.stop();
  }

  private loadInitialData() {
    this.chessService.getBoard().subscribe(data => this.state.updateFromDto(data));
    this.state.updateAdvantages();
  }

  onSquareClick(row: number, col: number) {
    if (this.state.isGameOver()) return;

    const clickedPiece = this.state.board()[row][col];
    const selected = this.state.selectedSquare();

    if (selected?.row === row && selected?.col === col) {
      this.state.selectedSquare.set(null);
      this.state.legalMoves.set([]);
      return;
    }

    if (selected && this.isLegalMove(row, col)) {
      this.chessService.movePiece(selected.row, selected.col, row, col).subscribe({
        next: (data: any) => {
          this.state.updateFromDto(data);
          this.state.updateAdvantages();

          if (data.isGameOver) {
            this.timer.gameStarted.set(false);
          } else {
            this.timer.switchTurn();
          }

          this.cdr.detectChanges();
        },
        error: () => this.handleBadMove(row, col)
      });
      return;
    }

    // 3. Zaznaczenie figury (tylko własnego koloru)
    if (clickedPiece !== "" && clickedPiece[0] === (this.timer.activeColor() === 'white' ? 'w' : 'b')) {
      this.state.selectedSquare.set({ row, col });
      this.chessService.getLegalMoves(row, col).subscribe(moves => {
        this.state.legalMoves.set(moves);
        this.cdr.detectChanges();
      });
    } else {
      this.state.selectedSquare.set(null);
      this.state.legalMoves.set([]);
    }
  }

  onNewGame() {
    if (!confirm("Czy na pewno chcesz rozpocząć nową grę?")) return;

    this.chessService.resetBoard().subscribe((data: any) => {
      this.state.reset(data);
      this.timer.reset();
      this.cdr.detectChanges();
    });
  }

  isKingInCheck(row: number, col: number): boolean {
    const p = this.state.board()[row][col];
    if (!p) return false;
    return (p === 'wK' && this.state.whiteInCheck()) || (p === 'bK' && this.state.blackInCheck());
  }

  isLegalMove = (r: number, c: number) => this.state.legalMoves().some(m => m.row === r && m.col === c);

  getPiecePath = (p: string) => p ? `/assets/chess-icons/${p}.svg` : '';

  handleBadMove(row: number, col: number) {
    this.badSquare = { row, col };
    setTimeout(() => {
      this.badSquare = null;
      this.cdr.detectChanges();
    }, 300);
  }
}
