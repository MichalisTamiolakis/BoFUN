import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameService } from 'src/app/global/services/game.service';

@Component({
  selector: 'app-trivia-answer-question',
  templateUrl: './trivia-answer-question.component.html',
  styleUrls: ['./trivia-answer-question.component.scss'],
})
export class TriviaAnswerQuestionComponent implements OnInit {
  playerId: string | undefined | null = '';
  Math: any;
  Object: any;
  minutes: number = 0;
  seconds: number = 0;
  public game: any;
  public options = [false,false,false,false]
  gameInfo:any = {
    question : "From which Shakespeare play is the line 'Now is the winter of our discontent'?",
    options: [
      "Hamlet",
      "Richard and Third",
      "King Lear",
      "Romeo and Juliet"
    ]
  }

  constructor(
    private route: ActivatedRoute,
    private gameService: GameService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.playerId = this.route.snapshot.paramMap.get('positionId');
    this.Math = Math;
    this.Object = Object;

    this.gameService.getGame().subscribe((result: any) => {
      this.minutes = Math.trunc(result.duration / 60);
      this.seconds = result.duration - this.minutes * 60;
    });
  }
  optionIsSelected(){
    for (let i = 0; i < this.options.length; i++) {
      if(this.options[i]===true) return true;
    }
    return false;
  }
}
