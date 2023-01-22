import { GameService } from 'src/app/global/services/game.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-end-game',
  templateUrl: './end-game.component.html',
  styleUrls: ['./end-game.component.scss']
})
export class EndGameComponent implements OnInit {
  winnerTeam:any
  constructor(private gameService:GameService,private router: Router) { }

  ngOnInit(): void {
    this.gameService.getWinnerTeam().subscribe((result:any)=>{
      this.winnerTeam = result;
      
      setTimeout(() => {
        this.router.navigateByUrl('surroundwall/statistics');
      }, 6000);
    })
  }

}
