
import { Team } from './../../global/models/team/team';
import { Component, Input, OnInit } from '@angular/core';
import { Player } from 'src/app/global/models/player/player';

@Component({
  selector: 'app-team-card',
  templateUrl: './team-card.component.html',
  styleUrls: ['./team-card.component.scss'],
})
export class TeamCardComponent implements OnInit {
  @Input('team') team: Team = {
    id: -1,
    name: '',
    image: '',
    members: [],
    color: '',
    sequence: [],
  };
  players: Array<Player> = [
    {
      id: 1,
      username: 'Alexandra',
      teamId: 1,
      image: '',
      positionId: 0,
    },
    {
      id: 2,
      username: 'Michalis',
      teamId: 1,
      image: '',
      positionId: 1,
    },
    // {
    //   id: 3,
    //   username: 'Kostas',
    //   teamId: 2,
    //   image: '',
    //   positionId: 2,
    // },
    // {
    //   id: 4,
    //   username: 'Zack',
    //   teamId: 2,
    //   image: '',
    //   positionId: 3,
    // },
  ]
  teamPlayers:any;
  constructor() {}

  ngOnInit(): void {
    this.teamPlayers = this.players.filter(({teamId})=> teamId === this.team.id)
    console.log(this.teamPlayers)
  }
}
