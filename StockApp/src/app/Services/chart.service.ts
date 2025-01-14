import { Injectable } from "@angular/core";
import { Interval } from "../Models/Interfaces/enums/interval-time.enum";
import { stockData } from "../Models/models/Stock-model";
import { environment } from "src/environments/environment.development";
import { ApiHttpService } from "./api-http.service";
import { Observable, Subject } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class ChartService {

    private loadedStockData: stockData[] = [];
    private stooqSubscription: Subject<stockData[]> = new Subject();

    constructor(private apiHttpService: ApiHttpService) { }

    public getStooqDateSubscribe(): Observable<stockData[]> {
        return this.stooqSubscription;
    }

    public getAPICompanyRequest(companyPrefix: string) {
        const model = {
            prefix: companyPrefix,
            interval: Interval.day
        }
        this.apiHttpService.post<stockData[]>(`${environment.apiUrl}Stooq/GetStockData`, model).subscribe(result => {
            this.loadedStockData = [];
            result.map(x => x.date = new Date(x.date));
            this.loadedStockData = result;
            this.stooqSubscription.next(this.loadedStockData)
        })
    }
}