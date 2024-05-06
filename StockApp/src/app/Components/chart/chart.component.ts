import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import { stockData } from 'src/app/Models/models/Stock-model';
import { Interval } from 'src/app/Models/Interfaces/enums/interval-time.enum';
import { ApiHttpService } from 'src/app/Services/api-http.service';
import { LoadCSVService } from 'src/app/Services/load-csv.service';
import { PatternService } from 'src/app/Services/pattern.service';
import { environment } from 'src/environments/environment.development';
import { ChartService } from 'src/app/Services/chart.service';
import { Indicators } from 'src/app/Models/Interfaces/enums/indicator-type.enum';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {

  public value = "text";
  public timeStock: { time: string, numberOfDays: number }[] = [{ time: '1M', numberOfDays: 20 }, { time: '2M', numberOfDays: 40 }, { time: '3M', numberOfDays: 60 }, { time: '6M', numberOfDays: 120 }, { time: '1R', numberOfDays: 240 }, { time: '3R', numberOfDays: 720 }, { time: 'MAX', numberOfDays: 99999 }];
  public quantityOfStockValue = this.timeStock[1].numberOfDays;
  public chart: any;
  public recordsStocks: stockData[] = [];

  private dateTimeArray: string[] = [];
  private stockValueArray: number[] = [];
  private averageRolling: any[] = [];
  public lengthArray: number[] = [10, 20, 30, 50];
  public lengthChartSMA: number = 10;
  public linesArray: { length: number, name: string }[] = [{ length: this.lengthChartSMA, name: 'SMA ' + this.lengthChartSMA }];
  private loadedStockData: stockData[] = [];
  public indicatorArray: string[] = Object.keys(Indicators).map(key => Indicators[key as keyof typeof Indicators]);
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
      this.checkPoints();
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
    this.populateStockDataArrays();
    this.averageRolling = this.calculateSMA(this.lengthChartSMA);
    this.createLineChart();
  }

  private checkPoints() {
    if (this.linesArray[0] === undefined)
      return;

    this.populateStockDataArrays();
    const sma = this.patternService.calculateAverageRolling(this.stockValueArray, this.linesArray[0].length);
    const points = this.patternService.detectChangePoints(sma)

    const dataStock = this.loadedStockData.slice(-1 * this.quantityOfStockValue);
    points.forEach(point => {
      console.log(dataStock[point + this.linesArray[0].length])
    });
  }

  public updateChart(lineChart: any) {
    if (lineChart.length < 1)
      return;
    const line = this.chart.data.datasets.find((x: any) => x.label == lineChart.name)
    if (line === undefined)
      return;

    const dataChart = this.calculateSMA(lineChart.length);
    line.data = dataChart;

    this.chart.update();
  }

  private addIndicator(indicator: string) {
    if (this.chart === undefined)
      return;
    const lengthSMA = 20;
    const labelName = indicator + ' ' + lengthSMA;
    this.linesArray.push({ length: lengthSMA, name: labelName })
    const dataChart = this.chooseIndicator(indicator, lengthSMA);

    this.chart.data.datasets.push({
      label: labelName,
      data: dataChart,
      borderColor: this.randomColor(),
      backgroundColor: this.randomColor(),
    });

    this.chart.update();
  }

  private randomColor(): string {
    return 'rgba(' +
      Math.floor(Math.random() * 256) + ',' +
      Math.floor(Math.random() * 256) + ',' +
      Math.floor(Math.random() * 256) + ',' +
      Math.random().toPrecision(2).slice(2, 4) + ')';
  }

  private chooseIndicator(indicator: string, length: number): number[] {
    switch (indicator) {
      case Indicators.SMA:
        return this.calculateSMA(length);
      case Indicators.WMA:
        return this.calculateWMA(length);
      case Indicators.EMA:
        return this.calculateEMA(length);
      default:
        return [];
    }
  }

  private calculateSMA(lengthSMA: number): number[] {
    const sma = this.patternService.calculateAverageRolling(this.stockValueArray, lengthSMA);
    sma.unshift(...new Array(lengthSMA).fill(null));
    return sma;
  }

  private calculateWMA(lengthWMA: number): number[] {
    return this.patternService.weightedMovingAverage(this.stockValueArray, lengthWMA);
  }

  private calculateEMA(lengthWMA: number): number[] {
    return this.patternService.exponentialMovingAverage(this.stockValueArray, lengthWMA);
  }

  public changeDateChart() {
    this.populateStockDataArrays();

    this.chart.data.labels = this.dateTimeArray;
    this.chart.data.datasets[0].data = this.stockValueArray;

    for (let line of this.linesArray) {
      this.updateChart(line);
    }

    this.chart.update();

    this.checkPoints();
  }

  private populateStockDataArrays() {
    const dataStock = this.loadedStockData.slice(-1 * this.quantityOfStockValue);
    this.dateTimeArray = dataStock.map(y => y.date.toLocaleDateString('pl-PL'));
    this.stockValueArray = dataStock.map(x => x.close);
  }

  public addIndicatorToChart(indicator: string) {
    this.selectedIndicator = this.selectedIndicator.length ? `${this.selectedIndicator}, ${indicator}` : indicator;

    this.addIndicator(indicator);
  }
}