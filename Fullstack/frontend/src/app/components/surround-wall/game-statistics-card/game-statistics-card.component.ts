import { TeamService } from './../../../global/services/team.service';
import { Component, Input, OnInit } from '@angular/core';
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart,
  ApexFill,
  ApexDataLabels,
  ApexLegend,
} from 'ng-apexcharts';
type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  fill: ApexFill;
  legend: ApexLegend;
  dataLabels: ApexDataLabels;
};
@Component({
  selector: 'app-game-statistics-card',
  templateUrl: './game-statistics-card.component.html',
  styleUrls: ['./game-statistics-card.component.scss'],
})
export class GameStatisticsCardComponent implements OnInit {
  chartOptions: ChartOptions | any;
  colors:any
  @Input('gamesScores') gamesScores: any
  constructor(private teamService:TeamService) {
    this.chartOptions = {
      series: [4, 6],
      labels: ['won', 'lost'],
      chart: {
        width: 230,
        type: 'donut',
      },
      dataLabels: {
        enabled: true,
      },
      fill: {
        type: 'gradient',
        colors: this.colors,
      },
      legend: {
        formatter: function (val: any, opts: any) {
          return val + ' - ' + opts.w.globals.series[opts.seriesIndex];
        },
      },
      responsive: [
        {
          breakpoint: 480,
          options: {
            chart: {
              width: 200,
            },
            legend: {
              position: 'bottom',
            },
          },
        },
      ],
    };
  }

  ngOnInit(): void {
    this.teamService.getTeams().subscribe((result:any)=>{
      this.colors= result.map((team:any)=> team.color)
      // console.log(colors)
    })
  }
}
