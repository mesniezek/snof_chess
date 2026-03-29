import { Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class GameTimerService {
  whiteTime = signal(600);
  blackTime = signal(600);
  currentTime = signal('');
  gameStarted = signal(false);
  activeColor = signal<'white' | 'black'>('white');

  private timerInterval: any;

  startClock() {
    if (this.timerInterval) return;
    this.timerInterval = setInterval(() => {
      this.updateSystemTime();
      if (this.gameStarted()) {
        this.tick();
      }
    }, 1000);
  }

  private updateSystemTime() {
    const now = new Date();
    this.currentTime.set(now.toLocaleTimeString('pl-PL', {
      hour: '2-digit', minute: '2-digit', second: '2-digit'
    }));
  }

  private tick() {
    if (this.activeColor() === 'white') {
      this.whiteTime.update(t => Math.max(0, t - 1));
    } else {
      this.blackTime.update(t => Math.max(0, t - 1));
    }
  }

  switchTurn() {
    this.activeColor.update(c => c === 'white' ? 'black' : 'white');
    this.gameStarted.set(true);
  }

  reset() {
    this.whiteTime.set(600);
    this.blackTime.set(600);
    this.activeColor.set('white');
    this.gameStarted.set(false);
  }

  format(seconds: number): string {
    const mins = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${mins.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  }

  stop() {
    if (this.timerInterval) clearInterval(this.timerInterval);
  }
}
