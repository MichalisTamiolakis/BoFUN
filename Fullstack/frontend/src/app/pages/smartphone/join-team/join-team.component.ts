import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Game } from 'src/app/global/models/game/game';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { TeamService } from 'src/app/global/services/team.service';
import { UserService } from 'src/app/global/services/user.service';

@Component({
  selector: 'app-join-team',
  templateUrl: './join-team.component.html',
  styleUrls: ['./join-team.component.scss'],
})
export class JoinTeamComponent implements OnInit {
  // playerName:any = ""
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
  url:string = '';
  image:boolean =false;
  // joinClicked: boolean = false;
  constructor(
    private route: ActivatedRoute,
    private userService: UserService,
    private teamService: TeamService,
    private router: Router,
    private sockets: SocketsService
  ) {}

  ngOnInit(): void {
    this.positionId = this.route.snapshot.paramMap.get('positionId');
    if (this.positionId !== null) {
      this.userService
        .create(Number.parseInt(this.positionId))
        .subscribe((result: any) => {
          console.log('player', result);
          this.playerName = result.username;
          this.url = result.image
          this.image = true
        });
    }
    this.teamService.getTeams().subscribe((result) => {
      this.teams = result;
      console.log(this.teams);
    });

    this.sockets.subscribe('TeamUpdated', (msg: any) => {
      console.log('LALALA', msg);
      // this.teams = JSON.parse(msg)
    });
    this.sockets.subscribe('PlayerUpdated', (msg: any) => {
      console.log('PlayerUpdated', msg);
      this.image = true
      // this.teams = JSON.parse(msg)
    });
  }

  joinTeam(teamId: number) {
    console.log('username', this.playerName);
    if (this.playerName !== '' && this.missingName === false)
      this.teamService
        .assignPlayerToTeam(
          Number.parseInt(this.positionId),
          teamId,
          this.playerName
        )
        .subscribe((result) => {
          this.router.navigateByUrl(
            'smartphone/reviewTeam/' + teamId + '/player/' + this.positionId
          );
        });
    else this.missingName = true;
  }

  keyPress(event: KeyboardEvent) {
    this.missingName = false;
    // const pattern = /[a-zA-Z0-9]/;
    // const inputChar = String.fromCharCode(event.charCode);
    // console.log('inputChar', inputChar);
    // if (!pattern.test(inputChar)) {
    //   // invalid character, prevent input
    //   this.missingName = true;
    //   // event.preventDefault();
    // } else this.missingName = false;
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file !== undefined) {
      const reader = new FileReader();
      console.log('file', file);
      reader.readAsDataURL(file);
      reader.onload = () => {
        const base64 = reader.result as string;
        this.userService.setAvatar(this.positionId,base64.split(',')[1]).subscribe((msg:any)=>{
          console.log("msg",msg)
          // var player = JSON.parse(msg)
          this.url = msg.image
          this.image = true
          console.log('this.url', this.url);
        })
        console.log('base64', base64);
        // do something with the base64 encoded string
      };
    }
  }

  // setPlayerName(event: any) {
  //   this.userService
  //     .setName(Number.parseInt(this.positionId), event.target.value.toString())
  //     .subscribe(() => {});
  // }
}
