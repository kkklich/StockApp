import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import { stockData } from 'src/app/Models/models/Stock-model';
import { Interval } from 'src/app/Models/Interfaces/enums/interval-time.enum';
import { ApiHttpService } from 'src/app/Services/api-http.service';
import { LoadCSVService } from 'src/app/Services/load-csv.service';
import { PatternService } from 'src/app/Services/pattern.service';
import { environment } from 'src/environments/environment.development';
import { ChartService } from 'src/app/Services/chart.service';
import { chartIndicatorModel } from 'src/app/Models/models/chart-indicator-model';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {

  public value = "text";
  public timeStock: { time: string, numberOfDays: number }[] = [{ time: '1M', numberOfDays: 20 }, { time: '2M', numberOfDays: 40 }, { time: '3M', numberOfDays: 60 }, { time: '6M', numberOfDays: 120 }, { time: '1R', numberOfDays: 240 }, { time: '3R', numberOfDays: 720 }, { time: 'MAX', numberOfDays: 99999 }];
  public quantityOfStockValue = this.timeStock[2].numberOfDays;
  public chart: any;
  public recordsStocks: stockData[] = [];

  private dateTimeArray: string[] = [];
  private stockValueArray: number[] = [];
  private averageRolling: any[] = [];
  public lengthArray: number[] = [10, 20, 30, 50];
  public lengthChartSMA: number = 10;

  public linesArray: chartIndicatorModel[] = [{ id: Math.random().toString(36).substr(2, 9), length: this.lengthChartSMA, name: 'SMA ' + this.lengthChartSMA }];
  private loadedStockData: stockData[] = [];
  public indicatorArray: string[] = ['SMA', 'EMA', 'ESA', 'WMA'];
  public selectedIndicator: string = '';

  constructor(private loadCSVService: LoadCSVService,
    private patternService: PatternService,
    private chartService: ChartService,
    private apiHttpService: ApiHttpService) {
    // this.getAPIRequest('pkn');
    // this.getAPIRequest('PKN');

    this.chartService.getStooqDateSubscribe().subscribe(res => {
      this.loadedStockData = res;
      this.applyChart();
    })
  }

  ngOnInit(): void {
    this.loadData();
  }

  private loadData() {
    this.loadCSVService.getRecordsSubscribe().subscribe(res => {
      this.loadedStockData = res;
      this.applyChart();
    })
  }


  private createLineChart(): void {
    const nameLine = "SMA " + this.lengthChartSMA;
    const fileName = this.loadCSVService.fileTitle;

    if (this.chart != undefined)
      this.chart.destroy();

    this.chart = new Chart("chart", {
      type: 'line',
      data: {
        labels: this.dateTimeArray,
        datasets: [{
          label: fileName,
          data: this.stockValueArray
        },
        {
          label: nameLine,
          data: this.averageRolling,
        }
        ],
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

  public applyChart() {
    const dataStock = this.loadedStockData.slice(-1 * this.quantityOfStockValue);

    this.dateTimeArray = dataStock.map(y => y.date.toLocaleDateString('pl-PL'));
    this.stockValueArray = dataStock.map(x => x.close);
    this.averageRolling = this.calculateSMA(this.lengthChartSMA);
    this.createLineChart();
  }

  public updateChart(lineChart: chartIndicatorModel) {
    if (lineChart.length < 1)
      return;
    const line = this.chart.data.datasets.find((x: any) => x.id == lineChart.id)
    if (line === undefined)
      return;

    const dataChart = this.chooseIndicator(lineChart);
    line.data = dataChart;

    this.chart.update();
  }

  private addIndicator(indicator: string) {
    if (this.chart === undefined)
      return;
    const length = 20;
    const id = Math.random().toString(36).substr(2, 9);
    this.linesArray.push(
      {
        id: id,
        length: length,
        name: indicator
      }
    )
    // const dataChart = this.chooseIndicator(indicator, lengthSMA);
    const dataChart = this.calculateSMA(length);

    this.chart.data.datasets.push({
      id: id,
      label: indicator + ' ' + length,
      data: dataChart
    });

    this.chart.update();
  }

  private chooseIndicator(indicator: chartIndicatorModel): number[] {
    switch (indicator.name) {
      case 'SMA':
        return this.calculateSMA(indicator.length);
      case 'WMA':
        return this.calculateWMA(indicator.length);
      case 'EMA':
        return this.calculateEMA(indicator.length);
      default:
        return [];
    }
  }

  private calculateSMA(lengthSMA: number): number[] {
    const sma = this.patternService.calculateAverageRolling(this.stockValueArray, lengthSMA);
    sma.unshift(...new Array(lengthSMA).fill(null));
    console.log(sma)
    return sma;
  }

  private calculateWMA(lengthWMA: number): number[] {
    return this.patternService.weightedMovingAverage(this.stockValueArray, lengthWMA);
  }

  private calculateEMA(lengthWMA: number): number[] {
    return this.patternService.exponentialMovingAverage(this.stockValueArray, lengthWMA);
  }

  public changeDateChart() {
    const dataStock = this.loadedStockData.slice(-1 * this.quantityOfStockValue);
    this.dateTimeArray = dataStock.map(y => y.date.toLocaleDateString('pl-PL'));
    this.stockValueArray = dataStock.map(x => x.close);

    this.chart.data.labels = this.dateTimeArray;
    this.chart.data.datasets[0].data = this.stockValueArray;

    for (let line of this.linesArray) {
      this.updateChart(line);
    }

    this.chart.update();
  }

  public addIndicatorToChart(indicator: string) {
    this.selectedIndicator = this.selectedIndicator.length ? `${this.selectedIndicator}, ${indicator}` : indicator;

    this.addIndicator(indicator);
  }

  protected removeIndicatorFromChart(lineChart: chartIndicatorModel) {
    const index = this.linesArray.indexOf(lineChart);
    if (index > -1) {
      this.linesArray.splice(index, 1);
      this.chart.data.datasets.splice(index + 1, 1);
      this.chart.update();
    } else {
      return;
    }
  }
}