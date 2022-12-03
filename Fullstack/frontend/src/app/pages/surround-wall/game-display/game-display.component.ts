import { Component, OnInit } from '@angular/core';
import { Player } from 'src/app/global/models/player/player';
import { Game } from 'src/app/global/models/game/game';
import { Team } from 'src/app/global/models/team/team';
import { MiniGame } from 'src/app/global/models/round/round';
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
      },
    ],
  };
  round = this.game.rounds[this.game.rounds.length-1];
  team : any = this.game.teams.find(({id}) => id === this.round.team) ;
  miniGame: string = (Object.values(MiniGame)[this.round.miniGame]).toString().toLowerCase();
  Math: any;
  Object: any;
  minutes: number = 0;
  seconds: number = 0;
  constructor() {
    this.Math = Math;
    this.Object = Object;
    this.minutes = Math.trunc(this.game.duration / 60);
    this.seconds = this.game.duration - this.minutes * 60;
    console.log((Object.values(MiniGame)[0]).toString().toLowerCase())
  }

  ngOnInit(): void {
    
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
