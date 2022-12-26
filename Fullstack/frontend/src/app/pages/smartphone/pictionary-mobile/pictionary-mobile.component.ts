import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameService } from 'src/app/global/services/game.service';
import { RoundService } from 'src/app/global/services/round.service';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';

@Component({
  selector: 'app-pictionary-mobile',
  templateUrl: './pictionary-mobile.component.html',
  styleUrls: ['./pictionary-mobile.component.scss'],
})
export class PictionaryMobileComponent implements OnInit {
  public gameInfo = {
    topic: 'A house and a tree',
  };
  Math: any;
  Object: any;
  minutes: number = 0;
  seconds: number = 0;
  currentRound: any;
  playerId:number;
  task:string='';
  constructor(
    private route: ActivatedRoute,
    private router: Router,
private sockets:SocketsService,
    private gameService: GameService,
    private roundService: RoundService
  ) {}
  public game: any;
  ngOnInit(): void {
    this.Math = Math;
    this.Object = Object;

    this.gameService.getGame().subscribe((result: any) => {
      this.minutes = Math.trunc(result.duration / 60);
      this.seconds = result.duration - this.minutes * 60;
    });
    this.roundService.getCurrentRound().subscribe((result: any) => {
      this.currentRound = result;
      let gameJson = JSON.parse(this.currentRound.minigameJSON);
      this.task = gameJson.task;
    });
    this.sockets.subscribe('NewRound', (msg: any) => {
      this.playerId = Number(this.route.snapshot.paramMap.get('playerId'));
      this.router.navigateByUrl('idle/' + this.playerId);
    });
  }

  onClick() {
    console.log('started', this.currentRound.started);
    if (!this.currentRound.started) this.startGame();
    else this.endGame();
  }

  startGame() {
    this.roundService
      .editCurrentRound(false, true, false)
      .subscribe((result: any) => {
        this.currentRound = result;
      });
  }

  endGame() {
    this.roundService
      .editCurrentRound(true, true, true)
      .subscribe((result: any) => {
        this.currentRound = result;
      });
  }
}
