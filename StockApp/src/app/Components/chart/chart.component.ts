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

  public timeStock: { time: string, numberOfDays: number }[] = [{ time: '1M', numberOfDays: 20 }, { time: '2M', numberOfDays: 40 }, { time: '3M', numberOfDays: 60 }, { time: '6M', numberOfDays: 120 }, { time: '1R', numberOfDays: 240 }, { time: '3R', numberOfDays: 720 }, { time: 'MAX', numberOfDays: 99999 }];
  public quantityOfStockValue = this.timeStock[1].numberOfDays;
  public chart: any;
  public recordsStocks: stockData[] = [];

  private dateTimeArray: string[] = [];
  private stockValueArray: number[] = [];
  private averageRolling: any[] = [];
  public lengthArray: number[] = [10, 20, 30, 50];
  public lengthChartSMA: number = 10;
  private loadedStockData: stockData[] = [];

  constructor(private loadCSVService: LoadCSVService,
    private patternService: PatternService) { }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.loadCSVService.getRecordsSubscribe().subscribe(res => {
      this.loadedStockData = res;
      this.applyChart();
    })
  }

  public applyChart() {
    const dataStock = this.loadedStockData.slice(-1 * this.quantityOfStockValue);
    this.dateTimeArray = dataStock.map(y => y.date.toLocaleDateString('pl-PL'));
    this.stockValueArray = dataStock.map(x => x.close);

    this.calculatePattern();
    this.createLineChart();
  }

  private calculatePattern() {
    this.averageRolling = this.patternService.calculateAverageRolling(this.stockValueArray, this.lengthChartSMA);
    this.averageRolling.unshift(...new Array(this.lengthChartSMA).fill(null));
  }


  private createLineChart(): void {
    if (this.chart != undefined)
      this.chart.destroy();

    const nameLine = "SMA " + this.lengthChartSMA;

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
          label: nameLine,
          data: this.averageRolling,
        }],
      },
      options: {
        responsive: true,
        elements: {
          point: {
            radius: 0
          }
        }
      }
    });
  }
}

