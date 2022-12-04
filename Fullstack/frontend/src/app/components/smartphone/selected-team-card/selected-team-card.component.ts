import { Component, Input, OnInit } from '@angular/core';
import { Player } from 'src/app/global/models/player/player';
import { Team } from 'src/app/global/models/team/team';

@Component({
  selector: 'app-selected-team-card',
  templateUrl: './selected-team-card.component.html',
  styleUrls: ['./selected-team-card.component.scss']
})
export class SelectedTeamCardComponent implements OnInit {
  @Input('team') team: Team = {
    id: -2,
    name: '',
    image: '',
    members: [],
    color: 'white',
  };
  @Input('players') players: Array<Player> = []
  constructor() { }

  ngOnInit(): void {
    console.log("team",this.team)
  }

}
