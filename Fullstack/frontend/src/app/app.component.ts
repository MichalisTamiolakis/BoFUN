import { Component } from '@angular/core';
import { trigger, transition, animate, style } from '@angular/animations';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  animations: [
    trigger('slideInOut', [
      transition(':enter', [
        style({ transform: 'translateX(100%)' }),
        animate('2s ease-in', style({ transform: 'translateX(0%)' }))
      ]),
      transition(':leave', [
        animate('2s ease-out', style({ transform: 'translateX(-100%)' }))
      ])
    ])
  ]
})
export class AppComponent {
  animationActive  = true
  title = 'frontend';
}
