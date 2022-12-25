import { Team } from '../../../global/models/team/team';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Player } from 'src/app/global/models/player/player';
import { TeamService } from 'src/app/global/services/team.service';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';

@Component({
  selector: 'app-team-card',
  templateUrl: './team-card.component.html',
  styleUrls: ['./team-card.component.scss'],
})
export class TeamCardComponent implements OnInit {
  @Output("emitTeamId") teamEmitter: EventEmitter<number> = new EventEmitter();
  @Input('team') team: Team = {
    id: -2,
    name: '',
    image: '',
    members: [],
    color: '',
  };
  players: Array<Player> = [];
  teamPlayers: any;
  constructor(
    private teamService: TeamService,
    private sockets: SocketsService
  ) {}

  ngOnInit(): void {
    this.teamService.getTeamPlayers(this.team.id).subscribe((result) => {
      this.teamPlayers = result;
      console.log(this.teamPlayers);
    });

    this.sockets.subscribe('TeamUpdated', (data: any) => {
      console.log("socket msg",JSON.parse(data))
      let team =JSON.parse(data)
      if(this.team.id === team.id){
        this.teamService.getTeamPlayers(this.team.id).subscribe((result) => {
          this.teamPlayers = result;
        });
      }
    });
  }

  emitTeamId(){
    this.teamEmitter.emit(this.team.id);
  }
}
