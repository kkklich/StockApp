import { Component, ViewChild } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { CompaniesTickerModel } from 'src/app/Models/models/Companies-prefix-model';
import { stockData } from 'src/app/Models/models/Stock-model';
import { ApiHttpService } from 'src/app/Services/api-http.service';
import { ChartService } from 'src/app/Services/chart.service';
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
  public selectedCompany: CompaniesTickerModel = { name: "PKN ORLEN", ticker: "PKN" }
  public companyList: CompaniesTickerModel[] = [];

  constructor(private loadCSV: LoadCSVService,
    private apiHttpService: ApiHttpService,
    private chartService: ChartService) {
    this.selectedMedia = this.companyArray[0];


    this.loadJSONFile('companies_WSE.json').then(res => {
      this.companyList = res;
    })
  }

  public dowloadCSV(event: any) {
    this.loadCSV.uploadDocument(event).then(res => {
      this.records = res;
    });
  }

  public changeCompany() {
    this.chartService.getAPICompanyRequest(this.selectedCompany.ticker);
  }

  public async loadJSONFile(fileName: string): Promise<Array<CompaniesTickerModel>> {
    return new Promise(async (resolve, reject) => {
      try {
        const data: Array<CompaniesTickerModel> = await firstValueFrom(this.apiHttpService.get(`/assets/${fileName}`));
        resolve(data);
      } catch (e) {
        reject(e);
      }
    });
  }
}
