import { TeamService } from 'src/app/global/services/team.service';
import { RoundService } from 'src/app/global/services/round.service';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { Component, OnInit, SimpleChanges } from '@angular/core';
import { Game } from 'src/app/global/models/game/game';
import { MiniGame } from 'src/app/global/models/round/round';
import { Router } from '@angular/router';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-pictionary',
  templateUrl: './pictionary.component.html',
  styleUrls: ['./pictionary.component.scss'],
})
export class PictionaryComponent implements OnInit {
  Math: any;
  minutes: number = 0;
  seconds: number = 0;
  currentRound: any;
  teamName: string = '';
  round: any;
  pictureSVG: string = '<svg></svg>';
  constructor(
    private sockets: SocketsService,
    private roundService: RoundService,
    private teamService: TeamService,
    private router: Router,
    private sanitizer: DomSanitizer
  ) {
    document.body.style.background = '#F9F8F2';
    this.Math = Math;
    // this.minutes = Math.trunc(this.game.duration / 60);
    // this.seconds = this.game.duration - this.minutes * 60;
  }

  ngOnInit(): void {

    this.sanitizer.bypassSecurityTrustHtml(this.pictureSVG);

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
      
      this.pictureSVG = msg;
      console.log("msg", this.pictureSVG)
    });
  }

}
