import { Component, OnInit } from '@angular/core';
import { MiniGame, Round } from 'src/app/global/models/round/round';
import { GameService } from 'src/app/global/services/game.service';
import { TeamService } from 'src/app/global/services/team.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent implements OnInit {
  currentTeamName:string = ''
  icons: any = [
    {
      class: 'fa6-solid:masks-theater',
      style: {
        width: '25px',
        height: '19px',
      },
    },
    {
      class: 'fxemoji:blackquestionmark',
      style: {
        width: '25px',
        height: '25px',
      },
    },
    {
      class: 'el:brush',
      style: {
        width: '20px',
        height: '20px',
      },
    },
  ];

  game: any = {
    duration: 120,
    totalPlayers: 8,
    players: [],
    teams: [
      {
        id: 0,
        name: 'Team 1',
        image: '',
        members: [],
        color: '#F2CA68',
      },
      {
        id: 1,
        name: 'Team 2',
        image: '',
        members: [],
        color: '#A663CC',
      },
      {
        id: 2,
        name: 'Team 3',
        image: '',
        members: [],
        color: '#6BBF59',
      },
      {
        id: 3,
        name: 'Team 4',
        image: '',
        members: [],
        color: '#6096BA',
      },
    ],
    pantomime: true,
    pictionary: true,
    trivia: true,
    sequence: [],
    winningTeam: -1,
    rounds: [],
  };
  rounds: any = [
    {
      team: 1, //which team is playing
      player: 1, // which player is playing
      miniGame: 0,
      victory: true,
      remainingTime: 0,
      started: true,
      ended: true,
    },
    {
      team: 3, //which team is playing
      player: 2, // which player is playing
      miniGame: 2,
      victory: false,
      remainingTime: 150,
      started: true,
      ended: true,
    },
    {
      team: 2, //which team is playing
      player: 2, // which player is playing
      miniGame: 1,
      victory: false,
      remainingTime: 150,
      started: false,
      ended: false,
    },
    {
      team: 1, //which team is playing
      player: 1, // which player is playing
      miniGame: 0,
      victory: true,
      remainingTime: 0,
      started: true,
      ended: true,
    },
    {
      team: 3, //which team is playing
      player: 2, // which player is playing
      miniGame: 2,
      victory: false,
      remainingTime: 150,
      started: true,
      ended: true,
    },
    {
      team: 2, //which team is playing
      player: 2, // which player is playing
      miniGame: 1,
      victory: false,
      remainingTime: 150,
      started: false,
      ended: false,
    },
    {
      team: 1, //which team is playing
      player: 1, // which player is playing
      miniGame: 0,
      victory: true,
      remainingTime: 0,
      started: true,
      ended: true,
    }
  ];
  groupedRounds: any = [];
  teamNames: Array<string> = [];
  emptyCells: any;
  constructor(
    private gameService: GameService,
    private teamService: TeamService
  ) {}

  ngOnInit(): void {
    this.gameService.getGame().subscribe(async (result) => {
      this.game = result;
      // this.teamName

      console.log(this.game);
      for (let i = 0; i < this.game.sequence.length; i++) {
        this.teamService
          .getTeam(this.game.sequence[i])
          .subscribe((team: any) => {
            this.teamNames.push(team.name);
          });
      }
      let test = [];
      this.rounds = this.game.rounds
      var currentTeamId = this.rounds[this.rounds.length-1].team;
      this.teamService
          .getTeam(currentTeamId)
          .subscribe((team: any) => {
            this.currentTeamName = team.name
          });
      while (this.rounds.length > 0) {
        test.push(this.rounds.splice(0, this.game.teams.length));
      }

      let lenth = this.toArray(test[test.length - 1]).length;
      for (let i = 0; i < this.game.teams.length - lenth; i++) {
        test[test.length - 1].push({
          team: -1, //which team is playing
          player: 2, // which player is playing
          miniGame: 1,
          victory: false,
          remainingTime: 150,
          started: false,
          ended: false,
        });
      }
      this.groupedRounds = test;
      console.log('this.groupedRounds', this.groupedRounds);
    });
  }

  getMiniGameName(id: number) {
    return Object.values(MiniGame)[id].toString();
  }

  async getTeamName(id: number) {
    var name = this.teamService.getTeam(id).subscribe((team: any) => {
      this.teamNames.push(team.name);
      console.log(this.teamNames);
    });
  }

  toArray(round: any) {
    // console.log("round=",typeof Object.keys(round).map(key => round[key]))
    return Object.keys(round).map((key) => round[key]);
  }
}
