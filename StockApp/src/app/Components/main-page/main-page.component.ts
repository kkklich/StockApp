import { Component, ViewChild } from '@angular/core';
import { stockData } from 'src/app/Models/Interfaces/Stock-model';
import { stockDataENG } from 'src/app/Models/Interfaces/Stock_ENG-model';
import { LoadCSVService } from 'src/app/Services/load-csv.service';
import { environment } from 'src/environments/environment.development';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {

  public companyArray: any[] = [{ name: "PKN ORLEN", symbol: "PKN" }, { name: "XTB", symbol: "XTB" }];
  public selectedMedia: any;

  public records: stockData[] = [];
  @ViewChild('csvReader') csvReader: any;

  constructor(private loadCSV: LoadCSVService) {
    this.selectedMedia = this.companyArray[0];

    console.log(environment.production)
  }

  changeCompany() {
  }

  public dowloadCSV() {

    this.loadCSV.getInfo();

  }

  uploadListener($event: any): void {

    let text = [];
    let files = $event.srcElement.files;

    if (this.isValidCSVFile(files[0])) {

      let input = $event.target;
      let reader = new FileReader();
      reader.readAsText(input.files[0]);

      reader.onload = () => {
        let csvData = reader.result;
        let csvRecordsArray = (<string>csvData).split(/\r\n|\n/);
        let headersRow = this.getHeaderArray(csvRecordsArray);

        this.records = this.getDataRecordsArrayFromCSVFile(csvRecordsArray, headersRow.length);
        console.log(this.records);
      };

      reader.onerror = function () {
        console.log('error is occured while reading file!');
      };

    } else {
      alert("Please import valid .csv file.");
      this.fileReset();
    }
  }


  getDataRecordsArrayFromCSVFile(csvRecordsArray: any, headerLength: any): any[] {

    let stockArray: any[] = [];

    for (let i = 1; i < csvRecordsArray.length; i++) {
      let curruntRecord = (<string>csvRecordsArray[i]).split(',');
      if (curruntRecord.length == headerLength) {
        const csvRecord = {
          date: curruntRecord[0].trim(),
          open: Number(curruntRecord[1].trim()),
          high: Number(curruntRecord[2].trim()),
          low: Number(curruntRecord[3].trim()),
          close: Number(curruntRecord[4].trim()),
          volumen: Number(curruntRecord[5].trim())
        };

        let newDate = new Date(curruntRecord[0].trim());
        console.log(newDate)

        stockArray.push(csvRecord);
      }
    }
    return stockArray;
  }

  isValidCSVFile(file: any) {
    return file.name.endsWith(".csv");
  }

  getHeaderArray(csvRecordsArr: any) {
    console.log(csvRecordsArr[0])
    let headers = (<string>csvRecordsArr[0]).split(',');
    let headerArray = [];
    for (let j = 0; j < headers.length; j++) {
      headerArray.push(headers[j]);
    }
    return headerArray;
  }

  fileReset() {
    this.csvReader.nativeElement.value = "";
    this.records = [];
  }


}
