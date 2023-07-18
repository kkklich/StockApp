import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import { stockData } from 'src/app/Models/Interfaces/Stock-model';
import { LoadCSVService } from 'src/app/Services/load-csv.service';
import { PatternService } from 'src/app/Services/pattern.service';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {

  private quantityOfStockValue = 100;
  public chart: any;
  public recordsStocks: stockData[] = [];

  private dateTimeArray: string[] = [];
  private stockValueArray: number[] = [];
  private averageRolling: number[] = [];

  constructor(private loadCSVService: LoadCSVService,
    private patternService: PatternService) { }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.loadCSVService.getRecordsSubscribe().subscribe(res => {
      const dataStock = res.slice(-1 * this.quantityOfStockValue);
      this.dateTimeArray = dataStock.map(x => x.date.toLocaleDateString('pl-PL'));

      this.stockValueArray = dataStock.map(x => x.close);
      this.calculatePattern();
      this.createLineChart();
    })

  }

  calculatePattern() {
    this.averageRolling = this.patternService.calculateAverageRolling(this.stockValueArray, 10);
    console.log(this.averageRolling);
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
        },
        {
          label: "xd",
          data: this.averageRolling,
        }],
      },
      options: {
        responsive: true
      }
    });
  }
}

