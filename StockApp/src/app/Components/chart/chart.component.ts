import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import { stockData } from 'src/app/Models/Interfaces/Stock-model';
import { LoadCSVService } from 'src/app/Services/load-csv.service';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {

  public chart: any;
  public recordsStocks: stockData[] = [];

  private dateTimeArray: any[] = [];
  private stockValueArray: any[] = [];
  constructor(private loadCSVService: LoadCSVService) { }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.loadCSVService.getRecordsSubscribe().subscribe(res => {
      this.dateTimeArray = res.map(x => x.date.toLocaleDateString('pl-PL'));
      this.stockValueArray = res.map(x => x.close.toString());
      this.createLineChart();
    })

  }

  private createLineChart(): void {
    if (this.chart != undefined)
      this.chart.destroy();

    const fileName = this.loadCSVService.fileTitle;
    this.chart = new Chart("chart", {
      type: 'line',
      data: {
        labels: this.dateTimeArray,
        datasets: [{
          label: fileName,
          data: this.stockValueArray,
        }],
      },
      options: {
        responsive: true
      }
    });
  }
}

