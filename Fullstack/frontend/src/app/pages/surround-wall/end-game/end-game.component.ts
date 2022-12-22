import { GameService } from 'src/app/global/services/game.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-end-game',
  templateUrl: './end-game.component.html',
  styleUrls: ['./end-game.component.scss']
})
export class EndGameComponent implements OnInit {
  winnerTeam:any
  constructor(private gameService:GameService) { }

  ngOnInit(): void {
    this.gameService.getWinnerTeam().subscribe((result:any)=>{
      this.winnerTeam = result;
    })
  }

}
