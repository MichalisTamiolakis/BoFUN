import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { TeamService } from 'src/app/global/services/team.service';
import { RoundService } from 'src/app/global/services/round.service';
import { Component, OnInit } from '@angular/core';
import { Player } from 'src/app/global/models/player/player';
import { Game } from 'src/app/global/models/game/game';
import { Team } from 'src/app/global/models/team/team';
import { MiniGame } from 'src/app/global/models/round/round';
import { Router } from '@angular/router';
@Component({
  selector: 'app-game-display',
  templateUrl: './game-display.component.html',
  styleUrls: ['./game-display.component.scss'],
})
export class GameDisplayComponent implements OnInit {
  
  game: Game = {
    duration: 10,
    totalPlayers: 4,
    players: [],
    teams: [{
      id: 1,
      name: 'Team 1',
      image: 'string',
      members: [1,2],
      color: 'purple',
      // sequence: [1,2], //seira paiktwn
    },{
      id: 2,
      name: 'Team 2',
      image: 'string',
      members: [3,4],
      color: 'yellow',
      // sequence: [3,4], //seira paiktwn
    }],
    pantomime: true,
    pictionary: true,
    trivia: true,
    sequence: [1, 2],
    winningTeam: -1,
    rounds: [
      {
        team: 1, //which team is playing
        player: 1, // which player is playing
        miniGame: 0,
        victory: false,
        remainingTime: 150,
        started: false,
        ended:false
      },
    ],
  };
  round :any;
  team : string = '' ;
  miniGame: string = ''
  //(Object.values(MiniGame)[this.round.miniGame]).toString().toLowerCase();
  Math: any;
  Object: any;
  minutes: number = 0;
  seconds: number = 0;
  currentRound:any;
  category:string='';
  constructor(private roundService:RoundService, private teamService:TeamService, private sockets:SocketsService,private router: Router) {
    this.Math = Math;
    this.Object = Object;
    // this.minutes = Math.trunc(this.game.duration / 60);
    // this.seconds = this.game.duration - this.minutes * 60;
    console.log((Object.values(MiniGame)[0]).toString().toLowerCase())
  }

  ngOnInit(): void {
    this.roundService.getCurrentRound().subscribe((result: any) => {
      this.round = result;
      this.teamService.getTeam(this.round.team).subscribe((team:any)=>{
        this.team = team.name;
      })
      let gameJson = JSON.parse(this.round.minigameJSON);
      this.category = gameJson.category;
      this.minutes = Math.trunc(this.round.remainingTime / 60);
    this.seconds = this.round.remainingTime - this.minutes * 60;
    console.log(this.round,this.minutes,this.seconds)
    });

    
    this.sockets.subscribe('RoundTimerUpdated', (msg: any) => {
      let round = JSON.parse(msg);
      this.round = round;
      this.minutes = Math.trunc(this.round.remainingTime / 60);
    this.seconds = this.round.remainingTime - this.minutes * 60;
    });

    this.sockets.subscribe('RoundEnded', (msg: any) => {
      let round = JSON.parse(msg);
      this.round = round;
      setTimeout(() => {
        this.router.navigateByUrl('surroundwall/main');
      }, 6000);
    });
    
  }

  startGame(){
    if(this.round.started === false){
    this.round.started = true;
    const timer = setInterval(() => {
      if (this.seconds === 0 && this.minutes === 0) {
        clearInterval(timer);
      } else if (this.seconds === 0) {
        this.seconds = 59;
        this.minutes--;
      } else this.seconds--;
      console.log(this.minutes, this.seconds);
    }, 1000);
  }}
}
