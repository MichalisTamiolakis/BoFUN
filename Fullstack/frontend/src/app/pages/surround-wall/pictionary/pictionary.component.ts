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
  }
}
