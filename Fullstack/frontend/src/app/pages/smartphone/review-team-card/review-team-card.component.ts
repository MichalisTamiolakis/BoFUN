import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Player } from 'src/app/global/models/player/player';
import { Team } from 'src/app/global/models/team/team';
import { TeamService } from 'src/app/global/services/team.service';
import { UserService } from 'src/app/global/services/user.service';

@Component({
  selector: 'app-review-team-card',
  templateUrl: './review-team-card.component.html',
  styleUrls: ['./review-team-card.component.scss'],
})
export class ReviewTeamCardComponent implements OnInit {
  teamId: string | null = '';
  playerId: string | null = '';
  team: any;
  players: any;
  constructor(private route: ActivatedRoute, private userService: UserService,
    private teamService: TeamService, private router: Router) {}

  ngOnInit(): void {
    this.playerId = this.route.snapshot.paramMap.get('playerId');
    this.teamId = this.route.snapshot.paramMap.get('teamId');
    console.log("teamId",this.teamId)
    this.teamService.getTeam(Number(this.teamId)).subscribe((team)=>{
      this.team = team;
      
      this.teamService.getTeamPlayers(Number(this.teamId)).subscribe((players)=>{
        this.players = players;
        console.log("team",this.team,"players",this.players);
      })
    })
  }

  removePlayer(){
    this.userService.removePlayerFromTeam(Number(this.playerId),Number(this.teamId)).subscribe((team)=>{
      this.router.navigateByUrl("smartphone/join/" + this.playerId);
    })
  }
}
