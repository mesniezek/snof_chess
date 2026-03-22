import {ChangeDetectorRef, Component, OnDestroy, OnInit} from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChessService } from './chess.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit, OnDestroy {
  board: string[][] = [];
  legalMoves: { row: number, col: number }[] = [];
  currentTime: string = '';
  whiteTime = 600;
  blackTime = 600;
  activeColor: 'white' | 'black' = 'white';
  gameStarted = false;
  timerInterval: any;
  whiteAdvantage = 0;
  blackAdvantage = 0;

  constructor(private chessService: ChessService, private cdr: ChangeDetectorRef) {}

  ngOnInit() {
    this.loadBoard();
    this.gameStarted = true;
    this.updateAdvantages();

    if (this.timerInterval) clearInterval(this.timerInterval);

    this.timerInterval = setInterval(() => {
      this.updateTime();

      if (this.gameStarted) {
        if (this.activeColor === 'white') {
          this.whiteTime = Math.max(0, this.whiteTime - 1);
        } else {
          this.blackTime = Math.max(0, this.blackTime - 1);
        }

        if (this.whiteTime === 0 || this.blackTime === 0) {
          this.gameStarted = false;
          alert("Koniec czasu!");
        }
      }

      this.cdr.detectChanges();
    }, 1000);
  }

  ngOnDestroy() {
    if (this.timerInterval) clearInterval(this.timerInterval);
  }

  formatTime(seconds: number): string {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  }

  updateTime() {
    const now = new Date();
    this.currentTime = now.toLocaleTimeString('pl-PL', {
      hour: '2-digit',
      minute: '2-digit',
      second: '2-digit'
    });
  }

  selectedSquare: { row: number, col: number } | null = null;
  badSquare: { row: number, col: number } | null = null;

  onSquareClick(row: number, col: number) {
    const clickedPiece = this.board[row][col];

    if (this.selectedSquare?.row === row && this.selectedSquare?.col === col) {
      this.selectedSquare = null;
      this.legalMoves = [];
      return;
    }

    if (this.selectedSquare && this.isLegalMove(row, col)) {
      this.chessService.movePiece(this.selectedSquare.row, this.selectedSquare.col, row, col).subscribe({
        next: (newBoard) => {
          this.board = [...newBoard];
          this.selectedSquare = null;
          this.legalMoves = [];

          this.gameStarted = true;
          this.updateAdvantages();
          this.activeColor = this.activeColor === 'white' ? 'black' : 'white';
          this.cdr.detectChanges();
        },
        error: () => this.handleBadMove(row, col)
      });
      return;
    }

    if (clickedPiece !== "") {
      const pieceColor = clickedPiece[0] === 'w' ? 'white' : 'black';
      if (pieceColor === this.activeColor) {
        this.selectedSquare = { row, col };
        this.legalMoves = [];
        this.chessService.getLegalMoves(row, col).subscribe(moves => {
          this.legalMoves = moves;
          this.cdr.detectChanges();
        });
      }
      return;
    }
    else {
      this.selectedSquare = null;
      this.legalMoves = [];
      this.cdr.detectChanges();
    }
  }

  handleBadMove(row: number, col: number) {
    this.badSquare = { row, col };
    this.selectedSquare = null;
    this.legalMoves = [];
    setTimeout(() => this.badSquare = null, 300);
  }

  getPiecePath(piece: string): string {
    if (!piece) return '';

    return `/assets/chess-icons/${piece}.svg`;
  }

  isLegalMove(row: number, col: number): boolean {
    return this.legalMoves.some(m => m.row === row && m.col === col);
  }

  loadBoard() {
    this.chessService.getBoard().subscribe(data => {
      this.board = [...data];
      this.cdr.detectChanges();
    });
  }

  onNewGame() {
    if (confirm("Czy na pewno chcesz rozpocząć nową grę?")) {
      this.chessService.resetBoard().subscribe({
        next: (newBoard) => {
          this.board = [...newBoard];

          this.whiteTime = 600;
          this.blackTime = 600;
          this.activeColor = 'white';
          this.gameStarted = false;
          this.selectedSquare = null;
          this.legalMoves = [];

          this.cdr.detectChanges();

          console.log("Gra zresetowana pomyślnie");
          this.gameStarted = true;
          this.whiteAdvantage = 0;
          this.blackAdvantage = 0;
        },
        error: (err) => console.error("Błąd podczas resetu gry:", err)
      });
    }
  }

  updateAdvantages() {
    this.chessService.getAdvantage('white').subscribe({
      next: (res: any) => {
        this.whiteAdvantage = res && typeof res === 'object' ? (res.value ?? res.advantage ?? 0) : (res ?? 0);
        this.cdr.detectChanges();
      },
      error: () => this.whiteAdvantage = 0
    });

    this.chessService.getAdvantage('black').subscribe({
      next: (res: any) => {
        this.blackAdvantage = res && typeof res === 'object' ? (res.value ?? res.advantage ?? 0) : (res ?? 0);
        this.cdr.detectChanges();
      },
      error: () => this.blackAdvantage = 0
    });
  }
}
