import { Component, Input, OnInit } from '@angular/core';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';

@Component({
  selector: 'app-trivia-option',
  templateUrl: './trivia-option.component.html',
  styleUrls: ['./trivia-option.component.scss']
})
export class TriviaOptionComponent implements OnInit {
  @Input('text') text: string = '';
  @Input('index') index: number = 0;
  @Input('options') options: any;
  public chosen:boolean = false
  public alphabet: Array<string> = ["A","B","C", "D"]
  constructor(private sockets:SocketsService) { }

  ngOnInit(): void {
  }

  chooseAnswer(){

    for (let i = 0; i < this.options.length; i++) {
      if(i===this.index) continue;
      this.options[i] = false;
      
    }
    this.options[this.index] = !this.options[this.index];

    this.sockets.publish("TriviaSelectedAnswerChanged", this.options[this.index]?this.index:-1);
  }

}
