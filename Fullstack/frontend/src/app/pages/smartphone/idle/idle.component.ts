import { MiniGame } from 'src/app/global/models/round/round';
import { SocketsService } from 'src/app/global/services/sockets/sockets.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-idle',
  templateUrl: './idle.component.html',
  styleUrls: ['./idle.component.scss'],
})
export class IdleComponent implements OnInit {
  playerId: number = -1;
  constructor(
    private route: ActivatedRoute,
    private sockets: SocketsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sockets.subscribe('NewRound', (msg: any) => {
      this.playerId = Number(this.route.snapshot.paramMap.get('playerId'));
      let round = JSON.parse(msg);
      if (round.player === this.playerId) {
        if (round.minigame === 0)
          this.router.navigateByUrl('smartphone/pantomime/' + round.player);
        else if (round.minigame === 1)
          this.router.navigateByUrl(
            'smartphone/trivia/' + round.player + '/hiddenQuestion'
          );
        else if (round.minigame === 2)
          this.router.navigateByUrl('smartphone/pictionary/' + round.player);
      }
      // this.teams = JSON.parse(msg)
    });
  }

  quit() {
    if (confirm('Are you sure you want to quit?') == true) {
      let text = 'You pressed OK!';
      console.log(text);
    } else {
      let text = 'You canceled!';
      console.log(text);
    }
  }
}
