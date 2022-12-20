import { Component, OnInit } from '@angular/core';
import { GameService } from 'src/app/global/services/game.service';

@Component({
  selector: 'app-pictionary-mobile',
  templateUrl: './pictionary-mobile.component.html',
  styleUrls: ['./pictionary-mobile.component.scss']
})
export class PictionaryMobileComponent implements OnInit {
  public gameInfo = {
    topic: 'A house and a tree',
  };
  Math: any;
  Object: any;
  minutes: number = 0;
  seconds: number = 0;
  constructor(private gameService: GameService) {}
  public game: any;
  ngOnInit(): void {
    this.Math = Math;
    this.Object = Object;

    this.gameService.getGame().subscribe((result: any) => {
      this.minutes = Math.trunc(result.duration / 60);
      this.seconds = result.duration - this.minutes * 60;
    });
  }

}
