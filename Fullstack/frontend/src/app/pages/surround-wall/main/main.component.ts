import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { MiniGame, Round } from 'src/app/global/models/round/round';
import { GameService } from 'src/app/global/services/game.service';
import { TeamService } from 'src/app/global/services/team.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
})
export class MainComponent implements OnInit {
  @ViewChild('tableContainer') private myScrollContainer: ElementRef;
  currentTeamName: string = '';
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

  game: any;
  rounds: any;
  groupedRounds: any = [];
  teamNames: Array<string> = [];
  emptyCells: any;
  nextTeamName: string = '';
  constructor(
    private gameService: GameService,
    private teamService: TeamService,
    private sockets: SocketsService,
    private router: Router
  ) {
    document.body.style.background = 'linear-gradient(354.41deg, #1C2020 5.35%, #2B2F2F 83.35%)';
  }

  ngOnInit(): void {
    this.teamService.getNextTeam().subscribe((res: any) => {
      this.currentTeamName = res.name;
    });
    this.teamService.getVeryNextTeam().subscribe((res: any) => {
      console.log("verNext=",res)
      this.nextTeamName = res.name;
    });
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
      this.rounds = this.game.rounds;
      while (this.rounds.length > 0) {
        test.push(this.rounds.splice(0, this.game.teams.length));
      }
      if (test.length > 0) {
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
        let elem = document.getElementById('table');
        let elem2 = document.getElementById('table-container');
        elem2.scrollTo(0, elem.offsetHeight);
      }
      try {
        setTimeout(() => {
          this.myScrollContainer.nativeElement.scrollTop =
            this.myScrollContainer.nativeElement.scrollHeight;
        }, 500);
      } catch (err) {}
      // console.log('this.groupedRounds', this.groupedRounds, elem.offsetHeight);
    });
    this.sockets.subscribe('CurrentRoundStarted', (msg: any) => {
      let round = JSON.parse(msg);
      console.log('round.minigame==', round.minigame);
      if (round.minigame === 0)
        this.router.navigateByUrl('surroundwall/pantomime');
      else if (round.minigame === 1)
        this.router.navigateByUrl('surroundwall/trivia');
      else if (round.minigame === 2)
        this.router.navigateByUrl('surroundwall/pictionary');

      // this.teams = JSON.parse(msg)
    });

    this.sockets.subscribe('GameOver', (msg: any) => {
      this.router.navigateByUrl('surroundwall/endGame');
    });
  }

  getMiniGameName(id: number) {
    return Object.values(MiniGame)[id].toString();
  }

  getTeamName(id: number) {
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
