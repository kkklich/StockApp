import { Injectable, ViewChild } from '@angular/core';
import { stockData } from '../Models/models/Stock-model';
import { Observable, Subject, Subscription } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class LoadCSVService {
  private records: stockData[] = [];
  public fileTitle: string = '';

  private recordsSubscription: Subject<stockData[]> = new Subject();
  @ViewChild('csvReader') csvReader: any;

  constructor() { }

  public getRecordsSubscribe(): Observable<stockData[]> {
    return this.recordsSubscription;
  }

  public getRecords(): stockData[] {
    return this.records;
  }

  public uploadDocument($event: any): Promise<stockData[]> {
    return new Promise((resolve, reject) => {
      const files = $event.srcElement.files;
      this.records = [];

      if (this.isValidCSVFile(files[0])) {
        let input = $event.target;
        const reader = new FileReader();
        reader.readAsText(input.files[0]);

        this.fileTitle = input.files[0].name;
        reader.onload = async () => {
          try {
            let csvData = reader.result;
            let csvRecordsArray = (<string>csvData).split(/\r\n|\n/);
            let headersRow = this.getHeaderArray(csvRecordsArray);

            this.records = this.getDataRecordsArrayFromCSVFile(csvRecordsArray, headersRow.length);
            this.recordsSubscription.next(this.records);
            resolve(this.records);
          } catch (err) {
            reject([]);
          }
        };

        reader.onerror = function () {
          console.log('error is occured while reading file!');
          reject([]);
        };

      } else {
        alert("Please import valid .csv file.");
        this.fileReset();
      }
    })
  }

  getDataRecordsArrayFromCSVFile(csvRecordsArray: any, headerLength: any): any[] {
    let stockArray: any[] = [];

    for (let i = 1; i < csvRecordsArray.length; i++) {
      let curruntRecord = (<string>csvRecordsArray[i]).split(',');
      if (curruntRecord.length == headerLength) {
        const csvRecord: stockData = {
          date: new Date(curruntRecord[0].trim()),
          open: Number(curruntRecord[1].trim()),
          high: Number(curruntRecord[2].trim()),
          low: Number(curruntRecord[3].trim()),
          close: Number(curruntRecord[4].trim()),
          volumen: Number(curruntRecord[5].trim())
        };
        stockArray.push(csvRecord);
      }
    }
    return stockArray;
  }

  isValidCSVFile(file: any) {
    return file.name.endsWith(".csv");
  }

  getHeaderArray(csvRecordsArr: any) {
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
