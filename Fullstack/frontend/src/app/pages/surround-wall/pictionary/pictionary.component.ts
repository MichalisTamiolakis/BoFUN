import { TeamService } from 'src/app/global/services/team.service';
import { RoundService } from 'src/app/global/services/round.service';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { Component, OnInit } from '@angular/core';
import { Game } from 'src/app/global/models/game/game';
import { MiniGame } from 'src/app/global/models/round/round';
import { Router } from '@angular/router';

@Component({
  selector: 'app-pictionary',
  templateUrl: './pictionary.component.html',
  styleUrls: ['./pictionary.component.scss'],
})
export class PictionaryComponent implements OnInit {
  // game: Game = {
  //   duration: 10,
  //   totalPlayers: 4,
  //   players: [],
  //   teams: [
  //     {
  //       id: 1,
  //       name: 'Team 1',
  //       image: 'string',
  //       members: [1, 2],
  //       color: 'purple',
  //       // sequence: [1, 2], //seira paiktwn
  //     },
  //     {
  //       id: 2,
  //       name: 'Team 2',
  //       image: 'string',
  //       members: [3, 4],
  //       color: 'yellow',
  //       // sequence: [3, 4], //seira paiktwn
  //     },
  //   ],
  //   pantomime: true,
  //   pictionary: true,
  //   trivia: true,
  //   sequence: [1, 2],
  //   winningTeam: -1,
  //   rounds: [
  //     {
  //       team: 1, //which team is playing
  //       player: 1, // which player is playing
  //       miniGame: 0,
  //       victory: false,
  //       remainingTime: 0,
  //       started: true,
  //       ended:false
  //     },
  //     {
  //       team: 2, //which team is playing
  //       player: 2, // which player is playing
  //       miniGame: 2,
  //       victory: false,
  //       remainingTime: 150,
  //       started: false,
  //       ended:false
  //     },
  //   ],
  // };
  // round = this.game.rounds[this.game.rounds.length - 1];
  // team: any = this.game.teams.find(({ id }) => id === this.round.team);
  // miniGame: string = Object.values(MiniGame)
  //   [this.round.miniGame].toString()
  //   .toLowerCase();
  Math: any;
  minutes: number = 0;
  seconds: number = 0;
  currentRound: any;
  teamName: string = '';
  round: any;
  base64:string = 'data:image/png;base64,';
  url:string = this.base64 
  // + 'iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAApgAAAKYB3X3/OAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAANCSURBVEiJtZZPbBtFFMZ/M7ubXdtdb1xSFyeilBapySVU8h8OoFaooFSqiihIVIpQBKci6KEg9Q6H9kovIHoCIVQJJCKE1ENFjnAgcaSGC6rEnxBwA04Tx43t2FnvDAfjkNibxgHxnWb2e/u992bee7tCa00YFsffekFY+nUzFtjW0LrvjRXrCDIAaPLlW0nHL0SsZtVoaF98mLrx3pdhOqLtYPHChahZcYYO7KvPFxvRl5XPp1sN3adWiD1ZAqD6XYK1b/dvE5IWryTt2udLFedwc1+9kLp+vbbpoDh+6TklxBeAi9TL0taeWpdmZzQDry0AcO+jQ12RyohqqoYoo8RDwJrU+qXkjWtfi8Xxt58BdQuwQs9qC/afLwCw8tnQbqYAPsgxE1S6F3EAIXux2oQFKm0ihMsOF71dHYx+f3NND68ghCu1YIoePPQN1pGRABkJ6Bus96CutRZMydTl+TvuiRW1m3n0eDl0vRPcEysqdXn+jsQPsrHMquGeXEaY4Yk4wxWcY5V/9scqOMOVUFthatyTy8QyqwZ+kDURKoMWxNKr2EeqVKcTNOajqKoBgOE28U4tdQl5p5bwCw7BWquaZSzAPlwjlithJtp3pTImSqQRrb2Z8PHGigD4RZuNX6JYj6wj7O4TFLbCO/Mn/m8R+h6rYSUb3ekokRY6f/YukArN979jcW+V/S8g0eT/N3VN3kTqWbQ428m9/8k0P/1aIhF36PccEl6EhOcAUCrXKZXXWS3XKd2vc/TRBG9O5ELC17MmWubD2nKhUKZa26Ba2+D3P+4/MNCFwg59oWVeYhkzgN/JDR8deKBoD7Y+ljEjGZ0sosXVTvbc6RHirr2reNy1OXd6pJsQ+gqjk8VWFYmHrwBzW/n+uMPFiRwHB2I7ih8ciHFxIkd/3Omk5tCDV1t+2nNu5sxxpDFNx+huNhVT3/zMDz8usXC3ddaHBj1GHj/As08fwTS7Kt1HBTmyN29vdwAw+/wbwLVOJ3uAD1wi/dUH7Qei66PfyuRj4Ik9is+hglfbkbfR3cnZm7chlUWLdwmprtCohX4HUtlOcQjLYCu+fzGJH2QRKvP3UNz8bWk1qMxjGTOMThZ3kvgLI5AzFfo379UAAAAASUVORK5CYII=';
  constructor(
    private sockets: SocketsService,
    private roundService: RoundService,
    private teamService: TeamService,
    private router: Router
  ) {
    document.body.style.background = 'none';
    this.Math = Math;
    // this.minutes = Math.trunc(this.game.duration / 60);
    // this.seconds = this.game.duration - this.minutes * 60;
  }

  ngOnInit(): void {
    this.roundService.getCurrentRound().subscribe((result: any) => {
      this.round = result;
      this.minutes = Math.trunc(this.round.remainingTime / 60);
      this.seconds = this.round.remainingTime - this.minutes * 60;

      this.teamService.getTeam(result.team).subscribe((team: any) => {
        this.teamName = team.name;
      });
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

    this.sockets.subscribe('GameOver', (msg: any) => {
      this.router.navigateByUrl('surroundwall/endGame');
    });

    this.sockets.subscribe('PictionaryDrawingUpdated', (msg: any) => {
      
      this.url = this.base64 + msg
      console.log("msg",msg,this.url)
    });
  }
}
