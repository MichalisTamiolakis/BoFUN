import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { map, Subscription, timer } from 'rxjs';
import { GameService } from 'src/app/global/services/game.service';
import { Gestures, LeapService } from 'src/app/global/services/leap.service';
// import { ApexChart } from "apexcharts";

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit {
    @ViewChild('column0ScrollableContainer', { static: true }) column0ScrollableContainer: ElementRef;
    @ViewChild('column1ScrollableContainer', { static: true }) column1ScrollableContainer: ElementRef;
    @ViewChild('column2ScrollableContainer', { static: true }) column2ScrollableContainer: ElementRef;
    
    chartOptions:any
    teamsScores:any
    gamesScores:any
    playersScores:any

    public selectedColumn:number = 2;

    constructor(private gameService: GameService, public leapService:LeapService) {
    }
    timerSubscription: Subscription;
    ngOnInit(): void {

        this.gameService.getTeamsScores().subscribe((result) => {
            this.teamsScores = result
        })
        this.gameService.getGamesScores().subscribe((result) => {
            this.gamesScores = result
        })
        this.gameService.getPlayersScores().subscribe((result) => {
            this.playersScores = result
        })

        this.leapService.gestureRecognizer().subscribe((gesture: Gestures) => {
            
            // Change column
            if (gesture === Gestures.SWIPE_LEFT) {
                // this.selectedColumn = Math.min(this.selectedColumn+1, 2);
                this.changeFocusedColumn(-1);
            }
            else if (gesture === Gestures.SWIPE_RIGHT) {
                this.changeFocusedColumn(+1);
            }

            // Scroll
            else if(gesture === Gestures.SWIPE_UP){
                this.scrollSelectedColumn(-120);
            }
            else if(gesture === Gestures.SWIPE_DOWN){
                this.scrollSelectedColumn(+120);
            }
        });

    }

    private changeFocusedColumn(columnChange:number){
        this.selectedColumn = Math.max(Math.min(this.selectedColumn + columnChange, 2),0);
    }

    private scrollSelectedColumn(amount:number){
        if(this.selectedColumn === 0){
            this.column0ScrollableContainer.nativeElement.scrollTop += amount;
        }
        else if(this.selectedColumn===1){
            this.column1ScrollableContainer.nativeElement.scrollTop += amount;
        }
        else if(this.selectedColumn===2){
            this.column2ScrollableContainer.nativeElement.scrollTop += amount;
        }
    }
}
