import { Component, OnInit } from '@angular/core';
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
  constructor() {
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
        colors: ['rgba(59, 183, 39, 1)', '#F44336'],
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

  ngOnInit(): void {}
}
