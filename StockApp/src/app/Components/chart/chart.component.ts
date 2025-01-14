import { Component, OnInit } from '@angular/core';
import Chart from 'chart.js/auto';
import { stockData } from 'src/app/Models/models/Stock-model';
import { ApiHttpService } from 'src/app/Services/api-http.service';
import { LoadCSVService } from 'src/app/Services/load-csv.service';
import { PatternService } from 'src/app/Services/pattern.service';
import { ChartService } from 'src/app/Services/chart.service';
import { chartIndicatorModel } from 'src/app/Models/models/chart-indicator-model';
import { Indicators } from 'src/app/Models/Interfaces/enums/indicator-type.enum';

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

  public linesArray: chartIndicatorModel[] = [];
  private loadedStockData: stockData[] = [];
  public indicatorArray: string[] = Object.keys(Indicators).map(key => Indicators[key as keyof typeof Indicators]);
  public selectedIndicator: string = '';
  public trend: string = '';
  public trendAverage: string = '';

  constructor(private loadCSVService: LoadCSVService,
    private patternService: PatternService,
    private chartService: ChartService,
    private apiHttpService: ApiHttpService) {

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

    this.addTwoMovingAverges();
  }

  public applyChart() {
    this.populateStockDataArrays();
    this.averageRolling = this.calculateSMA(this.lengthChartSMA);
    this.createLineChart();
    this.calculateIndicators();
  }

  private checkPoints() {
    if (this.linesArray[0] === undefined)
      return;

    this.populateStockDataArrays();
    const line = this.chart.data.datasets.find((x: any) => x.id === this.linesArray[0].id)
    if (line === undefined)
      return;
    const points = this.patternService.detectChangePoints(line.data);

    const dataStock = this.loadedStockData.slice(-1 * this.quantityOfStockValue);
  }

  public updateChart(lineChart: chartIndicatorModel) {
    if (lineChart.length < 1)
      return;
    const line = this.chart.data.datasets.find((x: any) => x.id == lineChart.id)
    if (line === undefined)
      return;

    const dataChart = this.chooseIndicator(lineChart.name, lineChart.length);
    line.data = dataChart;
    line.label = lineChart.name + ' ' + lineChart.length;

    this.chart.update();
    this.calculateIndicators();
  }

  private addIndicator(indicator: string, length: number = 20) {
    if (this.chart === undefined)
      return;
    const id = Math.random().toString(36).substr(2, 9);
    this.linesArray.push(
      {
        id: id,
        length: length,
        name: indicator
      }
    )
    const dataChart = this.chooseIndicator(indicator, length);
    const color = this.randomColor();

    this.chart.data.datasets.push({
      id: id,
      label: indicator + ' ' + length,
      data: dataChart,
      borderColor: color,
      backgroundColor: color
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
      case Indicators.RSI:
        return this.calculateRSI(length);
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
  private calculateRSI(length: number): number[] {
    console.log(this.stockValueArray, length)
    const xd = this.patternService.calculateRSI(this.stockValueArray, length);
    console.log(xd)
    return this.patternService.calculateRSI(this.stockValueArray, length);
  }

  public changeDateChart() {
    this.populateStockDataArrays();

    this.chart.data.labels = this.dateTimeArray;
    this.chart.data.datasets[0].data = this.stockValueArray;

    for (let line of this.linesArray) {
      this.updateChart(line);
    }

    this.chart.update();
    this.calculateIndicators();
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

  protected removeIndicatorFromChart(lineChart: chartIndicatorModel) {
    const index = this.linesArray.indexOf(lineChart);
    if (index < 0)
      return;

    this.linesArray.splice(index, 1);
    this.chart.data.datasets.splice(index + 1, 1);
    this.chart.update();
  }

  private calculateIndicators() {
    const line = this.chart.data.datasets.find((x: any) => x.id === this.linesArray[0].id)
    if (line === undefined)
      return;
    const lineChart = this.linesArray.find(x => x.id === this.linesArray[0].id)
    if (lineChart === undefined)
      return;
    this.trend = this.patternService.showTrend(line.data, lineChart.length);
    const upwardTrend = line.data[lineChart.length] < this.stockValueArray[this.stockValueArray.length - 1] ? true : false;
    this.trendAverage = 'Na podstawie średniej ' + line.label + '  trend jest ';
    this.trendAverage += upwardTrend ? 'wzrostowy' : 'spadkowy';
    this.checkPoints();
  }

  private addTwoMovingAverges() {
    this.addIndicator(Indicators.SMA, 20);
    this.addIndicator(Indicators.EMA, 20);
  }
}