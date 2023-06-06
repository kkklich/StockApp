import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import { stockData } from 'src/app/Models/Interfaces/Stock-model';
import { LoadCSVService } from 'src/app/Services/load-csv.service';
//or
// import Chart from 'chart.js';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {

  public chart: any;
  public recordsStocks: stockData[] = [];
  constructor(private loadCSVService: LoadCSVService) { }

  ngOnInit(): void {
    // this.createChart();
    this.loadData();
    this.createLineChart();
  }

  private loadData() {
    this.loadCSVService.getRecordsSubscribe().subscribe(res => {
      // console.log(res);
      const labelsChart = res.map(x => x.date.toLocaleDateString('pl-PL'));
      console.log(labelsChart);
    })

  }

  private createLineChart(): void {

    this.chart = new Chart("MyChart", {
      type: 'line', //this denotes tha type of chart

      data: {// values on X-Axis
        labels: ['2022-05-10', '2022-05-11', '2022-05-12', '2022-05-13',
          '2022-05-14', '2022-05-15', '2022-05-16', '2022-05-17',],
        datasets: [
          {
            label: "Sales",
            data: ['467', '576', '572', '79', '92',
              '574', '573', '576'],
            backgroundColor: 'blue'
          }
          // ,
          // {
          //   label: "Profit",
          //   data: ['542', '542', '536', '327', '17',
          //     '0.00', '538', '541'],
          //   backgroundColor: 'limegreen'
          // }
        ]
      },
      options: {
        aspectRatio: 2.5
      }
    });
  }


  createChart(): void {
    this.chart = new Chart("MyChart", {
      type: 'bar', //this denotes tha type of chart

      data: {// values on X-Axis
        labels: ['2022-05-10', '2022-05-11', '2022-05-12', '2022-05-13',
          '2022-05-14', '2022-05-15', '2022-05-16', '2022-05-17',],
        datasets: [
          {
            label: "Sales",
            data: ['467', '576', '572', '79', '92',
              '574', '573', '576'],
            backgroundColor: 'blue'
          },
          {
            label: "Profit",
            data: ['542', '542', '536', '327', '17',
              '0.00', '538', '541'],
            backgroundColor: 'limegreen'
          }
        ]
      },
      options: {
        aspectRatio: 2.5
      }

    });
  }

}
