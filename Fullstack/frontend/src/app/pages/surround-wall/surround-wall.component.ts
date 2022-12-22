import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Game } from 'src/app/global/models/game/game';
import { MiniGame } from 'src/app/global/models/round/round';

@Component({
  selector: 'app-surround-wall',
  templateUrl: './surround-wall.component.html',
  styleUrls: ['./surround-wall.component.scss'],
})
export class SurroundWallComponent implements OnInit {
  public days_of_the_week: Array<string> = [
    'Sunday',
    'Monday',
    'Tuesday',
    'Wendnesday',
    'Thursday',
    'Friday',
    'Saturday',
  ];
  public months: Array<string> = [
    'January',
    'February',
    'March',
    'April',
    'May',
    'June',
    'July',
    'August',
    'September',
    'October',
    'November',
    'December',
  ];
  public current_date_str: string ="";
  public current_time_str: string = "";

  isPictionary:boolean = false;
  constructor(private router: Router) {
    this.isPictionary = (this.router.url.split('/')).includes('pictionary')
  }

  ngOnInit() {
    console.log((this.router.url.split('/')).includes('pictionary'));
    setInterval(() => {
      const current_date = new Date();
      this.current_time_str =
        new Date().toLocaleTimeString();
      this.current_date_str =
        this.days_of_the_week[current_date.getDay()] +
        ' ' +
        current_date.getDate().toString() +
        ' ' +
        this.months[current_date.getMonth()];
    }, 1000);
  }
}
