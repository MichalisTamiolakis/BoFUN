import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Game } from 'src/app/global/models/game/game';
import { TeamService } from 'src/app/global/services/team.service';
import { UserService } from 'src/app/global/services/user.service';

@Component({
  selector: 'app-join-team',
  templateUrl: './join-team.component.html',
  styleUrls: ['./join-team.component.scss'],
})
export class JoinTeamComponent implements OnInit {
  // game: Game = {
  //   duration: 10,
  //   totalPlayers: 4,
  //   players: [
  //     {
  //       id: 1,
  //       username: 'Alexandra',
  //       teamId: 1,
  //       image: '',
  //       positionId: 0,
  //     },
  //     {
  //       id: 2,
  //       username: 'Michalis',
  //       teamId: 1,
  //       image: '',
  //       positionId: 1,
  //     },
  //     {
  //       id: 3,
  //       username: 'Kostas',
  //       teamId: 2,
  //       image: '',
  //       positionId: 2,
  //     },
  //     {
  //       id: 4,
  //       username: 'Zack',
  //       teamId: 2,
  //       image: '',
  //       positionId: 3,
  //     },
  //   ],
  //   teams: [
  //     {
  //       id: 1,
  //       name: 'Team 1',
  //       image: 'string',
  //       members: [1, 2],
  //       color: 'rgba(96, 150, 186, 1)',
  //       sequence: [1, 2], //seira paiktwn
  //     },
  //     {
  //       id: 2,
  //       name: 'Team 2',
  //       image: 'string',
  //       members: [3, 4],
  //       color: 'rgba(166, 99, 204, 1)',
  //       sequence: [3, 4], //seira paiktwn
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
  //       remainingTime: 150,
  //       started: false,
  //     },
  //   ],
  // };
  positionId: any;
  teams: any;
  playerName: string = '';
  missingName: boolean = false;
  namePass: boolean = false;
  // joinClicked: boolean = false;
  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private teamService: TeamService, private router: Router
  ) {}
 
  
  ngOnInit(): void {
    this.positionId = this.route.snapshot.paramMap.get('positionId');
    if (this.positionId !== null) {
      this.userService
        .create(Number.parseInt(this.positionId))
        .subscribe(() => {});
    }
    this.teamService.getTeams().subscribe((result) => {
      this.teams = result;
      console.log(this.teams);
    });
  }

  joinTeam(teamId: number) {
    console.log('username', this.playerName);
    if (this.playerName !== '' && this.missingName===false)
      this.teamService
        .assignPlayerToTeam(
          Number.parseInt(this.positionId),
          teamId,
          this.playerName
        )
        .subscribe((result) => {
          this.router.navigateByUrl("smartphone/reviewTeam/" + teamId + "/player/"+this.positionId);
        });
    else this.missingName = true
  }

  keyPress(event: KeyboardEvent) {
    const pattern = /[a-zA-Z0-9]/;
    const inputChar = String.fromCharCode(event.charCode);
    if (!pattern.test(inputChar)) {
        // invalid character, prevent input
        this.missingName=true;
        // event.preventDefault();
    }
    else this.missingName=false;
}

  // setPlayerName(event: any) {
  //   this.userService
  //     .setName(Number.parseInt(this.positionId), event.target.value.toString())
  //     .subscribe(() => {});
  // }
}
