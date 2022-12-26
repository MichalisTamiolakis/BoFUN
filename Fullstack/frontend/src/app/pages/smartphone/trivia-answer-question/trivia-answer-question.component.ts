import { RoundService } from 'src/app/global/services/round.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GameService } from 'src/app/global/services/game.service';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';

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
  currentRound:any
  public game: any;
  public options = [false,false,false,false]
  gameInfo:any = {
    question : "From which Shakespeare play is the line 'Now is the winter of our discontent'?",
    options: [
      "Hamlet",
      "Richard and Third",
      "King Lear",
      "Romeo and Juliet"
    ],
    correctAnswer: 0
  }

  constructor(
private sockets:SocketsService,
    private route: ActivatedRoute,
    private gameService: GameService,
    private router: Router,
    private roundService: RoundService
  ) {}

  ngOnInit(): void {
    this.playerId = this.route.snapshot.paramMap.get('playerId');
    this.Math = Math;
    this.Object = Object;

    this.gameService.getGame().subscribe((result: any) => {
      this.minutes = Math.trunc(result.duration / 60);
      this.seconds = result.duration - this.minutes * 60;
    });

    this.roundService.getCurrentRound().subscribe((result: any) => {
      this.currentRound = result;
      let gameJson = JSON.parse(this.currentRound.minigameJSON);
      this.gameInfo.question = gameJson.question;
      this.gameInfo.options = gameJson.answers;
      this.gameInfo.correctAnswer = gameJson.correctAnswer;
      this.roundService.editCurrentRound(false, true,false).subscribe();
    });

    this.sockets.subscribe('NewRound', (msg: any) => {
      this.router.navigateByUrl('idle/' + this.playerId);
    });
  }
  optionIsSelected(){
    for (let i = 0; i < this.options.length; i++) {
      if(this.options[i]===true) return true;
    }
    return false;
  }

  checkAnswer(){
    for (let i = 0; i < this.options.length; i++) {
      if(this.options[i]===true){
        if(i===this.gameInfo.correctAnswer){
          this.roundService.editCurrentRound(true, true,true).subscribe();
        }
        else this.roundService.editCurrentRound(false, true,true).subscribe();
      }
    }
  }
}
