import { Component, ViewChild } from '@angular/core';
import { stockData } from 'src/app/Models/Interfaces/Stock-model';
import { LoadCSVService } from 'src/app/Services/load-csv.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {

  public companyArray: any[] = [{ name: "PKN ORLEN", symbol: "PKN" }, { name: "XTB", symbol: "XTB" }];
  public selectedMedia: any;
  public records: stockData[] = [];

  constructor(private loadCSV: LoadCSVService) {
    this.selectedMedia = this.companyArray[0];
  }

  public dowloadCSV(event: any) {
    this.loadCSV.uploadDocument(event).then(res => {
      this.records = res;
      console.log(res);
    });
  }
}
