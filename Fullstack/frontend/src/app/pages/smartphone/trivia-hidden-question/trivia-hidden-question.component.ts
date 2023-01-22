import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameService } from 'src/app/global/services/game.service';

@Component({
  selector: 'app-trivia-hidden-question',
  templateUrl: './trivia-hidden-question.component.html',
  styleUrls: ['./trivia-hidden-question.component.scss']
})
export class TriviaHiddenQuestionComponent implements OnInit {
  playerId:string | undefined | null = ''
  
  
  Math: any;
  Object: any;
  minutes: number = 0;
  seconds: number = 0;
  constructor(private route: ActivatedRoute,private gameService: GameService,private router: Router) {}
  public game: any;
  ngOnInit(): void {
    this.playerId = this.route.snapshot.paramMap.get('playerId');
    this.Math = Math;
    this.Object = Object;

    this.gameService.getGame().subscribe((result: any) => {
      this.minutes = Math.trunc(result.duration / 60);
      this.seconds = result.duration - this.minutes * 60;
    });
  }

  revealQuestion(){
    this.router.navigateByUrl("smartphone/trivia/" + this.playerId +"/answerQuestion");
  }

}
