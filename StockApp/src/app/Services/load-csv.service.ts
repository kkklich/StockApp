import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';


@Injectable({
  providedIn: 'root'
})
export class LoadCSVService {

  private covidData: string = 'https://www.cdc.gov/coronavirus/2019-ncov/map-data-cases.csv';

  constructor(private http: HttpClient) { }

  public async getInfo() {
    const xd = await this.http.get(this.covidData, { responseType: 'text' });
    console.log(xd);

    return xd;
  }

}
