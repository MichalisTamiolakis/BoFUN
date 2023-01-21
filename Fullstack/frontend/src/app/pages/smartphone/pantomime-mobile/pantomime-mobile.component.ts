import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { RoundService } from './../../../global/services/round.service';
import { Component, OnInit } from '@angular/core';
import { GameService } from 'src/app/global/services/game.service';
import { ActivatedRoute, Router } from '@angular/router';
interface IObjectKeys {
  [key: string]: string | number | undefined;
}
@Component({
  selector: 'app-pantomime-mobile',
  templateUrl: './pantomime-mobile.component.html',
  styleUrls: ['./pantomime-mobile.component.scss'],
})

export class PantomimeMobileComponent implements OnInit {
  
  public gameInfo = {
    category: 'Movies',
    topic: 'Avengers: Age of Ultron',
  };
  public icons:IObjectKeys = {
    Movies : "bxs:movie-play",
    Book : "material-symbols:menu-book-outline-rounded"
  }
  Math: any;
  Object: any;
  minutes: number = 0;
  seconds: number = 0;
  currentRound:any
  playerId:number;
  constructor(private route: ActivatedRoute,
    private router: Router,private gameService: GameService,private roundService: RoundService,private sockets:SocketsService) {}
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
      this.gameInfo.topic = gameJson.task;
      this.gameInfo.category = gameJson.category;
      console.log("this.currentRound=",this.currentRound)
    });

    // this.sockets.subscribe('NewRound', (msg: any) => {
    //   this.playerId = Number(this.route.snapshot.paramMap.get('playerId'));
    //   this.router.navigateByUrl('idle/' + this.playerId);
    // });

    this.sockets.subscribe('RoundTimerUpdated', (msg: any) => {
      let round = JSON.parse(msg);
      this.currentRound = round;
      this.minutes = Math.trunc(this.currentRound.remainingTime / 60);
    this.seconds = this.currentRound.remainingTime - this.minutes * 60;
    });

    this.sockets.subscribe('RoundEnded', (msg: any) => {
      this.playerId = Number(this.route.snapshot.paramMap.get('playerId'));
      this.router.navigateByUrl('idle/' + this.playerId);
    });
    
  }

  

  onClick(){
    console.log("started",this.currentRound.started)
    if(!this.currentRound.started) this.startGame();
    else this.endGame();
  }

  startGame(){
    this.roundService.startCurrentRound().subscribe((result:any)=>{
      this.currentRound = result
      
    });
  }

  endGame(){
    this.roundService.setResult(true).subscribe((result:any)=>{
      this.currentRound = result
      console.log("endGame",this.currentRound)
      this.playerId = Number(this.route.snapshot.paramMap.get('playerId'));
      this.router.navigateByUrl('idle/' + this.playerId);
    });
  }

  // startGame() {
  //   if (this.round.started === false) {
  //     this.round.started = true;
  //     const timer = setInterval(() => {
  //       if (this.seconds === 0 && this.minutes === 0) {
  //         clearInterval(timer);
  //       } else if (this.seconds === 0) {
  //         this.seconds = 59;
  //         this.minutes--;
  //       } else this.seconds--;
  //       console.log(this.minutes, this.seconds);
  //     }, 1000);
  //   }
  // }
}
