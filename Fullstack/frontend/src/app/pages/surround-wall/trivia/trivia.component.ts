import { TeamService } from 'src/app/global/services/team.service';
import { RoundService } from 'src/app/global/services/round.service';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-trivia',
  templateUrl: './trivia.component.html',
  styleUrls: ['./trivia.component.scss'],
})
export class TriviaComponent implements OnInit {
  round: any;
  correct:string='';
  question: string = '';
  answers: Array<string> = [];
  teamName: string = '';
  minutes: number = 0;
  seconds: number = 0;
  selectedAnswer: number = -1;
  Math:any
  gameJson:any
  constructor(
    private sockets: SocketsService,
    private roundService: RoundService,
    private teamService: TeamService,private router: Router
  ) {
    this.Math = Math;
  }

  ngOnInit(): void {
    this.roundService.getCurrentRound().subscribe((result: any) => {
      this.round = result;
      this.minutes = Math.trunc(this.round.remainingTime / 60);
      this.seconds = this.round.remainingTime - this.minutes * 60;
      this.gameJson = JSON.parse(result.minigameJSON);
      this.question = this.gameJson.question;
      this.answers = this.gameJson.answers;
      this.correct = this.answers[this.gameJson.correctAnswer]
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

    this.sockets.subscribe('TriviaSelectedAnswerChanged', (msg: any) => {
      
      this.selectedAnswer = msg
      console.log("selectedAnswer",this.selectedAnswer)
    });

    this.sockets.subscribe('RoundEnded', (msg: any) => {
      let round = JSON.parse(msg);
      this.round = round;
      setTimeout(() => {
        this.router.navigateByUrl('surroundwall/main');
      }, 9000);
    });

    this.sockets.subscribe('GameOver', (msg: any) => {
      this.router.navigateByUrl('surroundwall/endGame');
    });
    // this.sockets.subscribe('NewRound', (msg: any) => {
    //   this.router.navigateByUrl('idle/' + this.playerId);
    // });
  }
}
