import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { Player } from 'src/app/global/models/player/player';
import { Team } from 'src/app/global/models/team/team';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { TeamService } from 'src/app/global/services/team.service';

@Component({
  selector: 'app-selected-team-card',
  templateUrl: './selected-team-card.component.html',
  styleUrls: ['./selected-team-card.component.scss'],
})

export class SelectedTeamCardComponent implements OnInit {
  @ViewChild('myInput', {static: true}) myInput: ElementRef;
  @Input('team') team: Team = {
    id: -2,
    name: '',
    image: '',
    members: [],
    color: 'white',
  };
  @Input('players') players: Array<Player> = [];
  editName: boolean = false;
  missingName: boolean = false;
  teamName: string = this.team.name;
  shouldFocus = true;
  constructor(private teamService:TeamService) {}

  ngOnInit(): void {
    console.log('team', this.team);
    this.teamName = this.team.name
  }

  toggleEditName() {
    this.editName = !this.editName;
    // if(!this.editName){
    //   this.shouldFocus = true;
    // }
    // else{
    //   setTimeout(()=>{this.myInput.nativeElement.focus();},1000)
    // }
    // this.teamInput.nativeElement.focus();
  }

  // ngAfterViewChecked() {
  //   if (this.shouldFocus && this.myInput) {
  //     this.myInput.nativeElement.focus();
  //     this.shouldFocus = false;
  //   }
  // }

  keyPress(value: string) {
    this.missingName = false;
    // const pattern = /[a-zA-Z0-9]/;
    // // const inputChar = String.fromCharCode(event.charCode);
    // console.log('pattern', pattern.test(value));
    // if (!pattern.test(value)) {
    //   // invalid character, prevent input
    //   this.missingName = true;
    //   // event.preventDefault();
    // } else this.missingName = false;
  }


  keyEnter(value: string) {
    console.log('key enter =', value);
    if (!this.missingName) {
      if(value==='') value = this.team.name
      this.teamService.editTeam(this.team.id,value,this.team.image).subscribe()
      this.toggleEditName();
    }
  }
}
