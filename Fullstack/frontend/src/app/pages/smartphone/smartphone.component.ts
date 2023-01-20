import { Component, OnInit } from '@angular/core';
import { trigger, transition, animate, style } from '@angular/animations';
// import { slideInLeft, slideOutLeft } from '@angular/animations';
@Component({
  selector: 'app-smartphone',
  templateUrl: './smartphone.component.html',
  styleUrls: ['./smartphone.component.scss'],
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
export class SmartphoneComponent implements OnInit {
  animationActive  = true
  constructor() { }

  ngOnInit(): void {
  }

}
