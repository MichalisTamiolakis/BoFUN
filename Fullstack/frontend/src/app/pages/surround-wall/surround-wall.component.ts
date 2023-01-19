import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Game } from 'src/app/global/models/game/game';
import { MiniGame } from 'src/app/global/models/round/round';
import { RoundService } from 'src/app/global/services/round.service';
import { GameService } from 'src/app/global/services/game.service';

@Component({
  selector: 'app-surround-wall',
  templateUrl: './surround-wall.component.html',
  styleUrls: ['./surround-wall.component.scss'],
})
export class SurroundWallComponent implements OnInit {
  public days_of_the_week: Array<string> = [
    'Sunday',
    'Monday',
    'Tuesday',
    'Wendnesday',
    'Thursday',
    'Friday',
    'Saturday',
  ];
  public months: Array<string> = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ];
  public current_date_str: string = '';
  public current_time_str: string = '';
  public round: any;
  isPictionary: boolean = false;
  winner: number = -1;
  constructor(
    private router: Router,
    private sockets: SocketsService,
    private roundService: RoundService,
    private gameService: GameService
  ) {
    this.isPictionary = this.router.url.split('/').includes('pictionary');
  }

  ngOnInit() {
    this.gameService.getWinnerTeam().subscribe((result: any) => {
      console.log('win', result);
      this.winner = result.id;
    });
    this.roundService.getCurrentRound().subscribe((result: any) => {
      this.round = result;
    });
    this.sockets.subscribe('RoundEnded', (msg: any) => {
      let round = JSON.parse(msg);
      this.round = round;
    });
    this.sockets.subscribe('NewRound', (msg: any) => {
      let round = JSON.parse(msg);
      this.round = round;
    });
    this.sockets.subscribe('GameOver', (msg: any) => {
      let team = JSON.parse(msg);
      this.winner = team.id;
    });
    this.sockets.subscribe('GameStarted', (msg: any) => {
      this.router.navigateByUrl('surroundwall/main');
    });
    this.isPictionary = this.router.url.split('/').includes('pictionary');
    console.log(this.round);
    setInterval(() => {
      const current_date = new Date();
      this.current_time_str = new Date().toLocaleTimeString();
      this.current_date_str =
        this.days_of_the_week[current_date.getDay()] +
        ' ' +
        current_date.getDate().toString() +
        ' ' +
        this.months[current_date.getMonth()];
    }, 1000);
  }
}
