import { Component, Input, OnInit } from '@angular/core';
import { MiniGame } from 'src/app/global/models/round/round';

@Component({
  selector: 'app-player-statistics-card',
  templateUrl: './player-statistics-card.component.html',
  styleUrls: ['./player-statistics-card.component.scss']
})
export class PlayerStatisticsCardComponent implements OnInit {
  
  @Input('playerScores') playerScores: any
  constructor() { }

  ngOnInit(): void {
  }

  getMiniGameName(id: number) {
    return Object.values(MiniGame)[id].toString();
  }

}
