import { Component, Input, OnInit } from '@angular/core';
import {
  ApexNonAxisChartSeries,
  ApexResponsive,
  ApexChart,
  ApexFill,
  ApexDataLabels,
  ApexLegend
} from "ng-apexcharts";

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
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.scss']
})
export class ChartComponent implements OnInit {
  @Input('miniGame') miniGame: any
  chartOptions:ChartOptions | any;
constructor() { 
    this.chartOptions = {
    series: [4, 6],
    labels: ['won', 'lost'],
    chart: {
      width: 200,
      type: "donut"
    },
    dataLabels: {
      enabled: true
    },
    fill: {
      type: "gradient",
      colors: ['rgba(59, 183, 39, 1)', '#F44336']
    },
    legend: {
      formatter: function(val:any, opts:any) {
        return val + " - " + opts.w.globals.series[opts.seriesIndex];
      }
    },
    responsive: [
      {
        breakpoint: 480,
        options: {
          chart: {
            width: 200
          },
          legend: {
            position: "bottom"
          }
        }
      }
    ]
  };}

  ngOnInit(): void {
  }

}
