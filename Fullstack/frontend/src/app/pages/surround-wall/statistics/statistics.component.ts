import { Component, OnInit } from '@angular/core';
import { GameService } from 'src/app/global/services/game.service';
// import { ApexChart } from "apexcharts";

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {
  chartOptions:any
  teamsScores:any
  gamesScores:any

  public selected:number = 0;

  constructor(private gameService: GameService, /*private leapService:LeapService*/) { 
    

  }

  ngOnInit(): void {
    this.gameService.getTeamsScores().subscribe((result) => {
      this.teamsScores = result
    })
    this.gameService.getGamesScores().subscribe((result) => {
      this.gamesScores = result
    })

  }

}
