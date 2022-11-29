import { Component, OnInit } from '@angular/core';
import { Game } from 'src/app/global/models/game/game';
import { MiniGame } from 'src/app/global/models/round/round';

@Component({
  selector: 'app-pictionary',
  templateUrl: './pictionary.component.html',
  styleUrls: ['./pictionary.component.scss'],
})
export class PictionaryComponent implements OnInit {
  game: Game = {
    duration: 10,
    totalPlayers: 4,
    players: [],
    teams: [
      {
        id: 1,
        name: 'Team 1',
        image: 'string',
        members: [1, 2],
        color: 'purple',
        sequence: [1, 2], //seira paiktwn
      },
      {
        id: 2,
        name: 'Team 2',
        image: 'string',
        members: [3, 4],
        color: 'yellow',
        sequence: [3, 4], //seira paiktwn
      },
    ],
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
        remainingTime: 0,
        started: true,
      },
      {
        team: 2, //which team is playing
        player: 2, // which player is playing
        miniGame: 2,
        victory: false,
        remainingTime: 150,
        started: false,
      },
    ],
  };
  round = this.game.rounds[this.game.rounds.length - 1];
  team: any = this.game.teams.find(({ id }) => id === this.round.team);
  miniGame: string = Object.values(MiniGame)
    [this.round.miniGame].toString()
    .toLowerCase();
  Math: any;
  minutes: number = 0;
  seconds: number = 0;

  constructor() {
    document.body.style.background = 'none';
    this.Math = Math;
    this.minutes = Math.trunc(this.game.duration / 60);
    this.seconds = this.game.duration - this.minutes * 60;
  }

  ngOnInit(): void {}
}
