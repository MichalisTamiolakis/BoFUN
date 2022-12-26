import { TeamService } from 'src/app/global/services/team.service';
import { RoundService } from 'src/app/global/services/round.service';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-trivia',
  templateUrl: './trivia.component.html',
  styleUrls: ['./trivia.component.scss']
})
export class TriviaComponent implements OnInit {
  question:string=''
  answers:Array<string> = []
  teamName:string = ''
  constructor(private sockets: SocketsService, private roundService:RoundService,private teamService:TeamService) { }

  ngOnInit(): void {
    this.roundService.getCurrentRound().subscribe((result:any)=>{
      let gameJson = JSON.parse(result.minigameJSON)
      this.question = gameJson.question
      this.answers = gameJson.answers
      this.teamService.getTeam(result.team).subscribe((team:any)=>{
        this.teamName = team.name;
      })
    })
    // this.sockets.subscribe('NewRound', (msg: any) => {
    //   this.router.navigateByUrl('idle/' + this.playerId);
    // });
  }

}
