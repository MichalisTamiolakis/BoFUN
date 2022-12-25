import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-idle',
  templateUrl: './idle.component.html',
  styleUrls: ['./idle.component.scss']
})
export class IdleComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  quit(){
    if (confirm("Are you sure you want to quit?") == true) {
      let text = "You pressed OK!";
      console.log(text)
    } else {
      let text = "You canceled!";
      console.log(text)
    }
  }

}
