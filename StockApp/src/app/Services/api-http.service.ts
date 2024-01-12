import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class ApiHttpService {

    constructor(private http: HttpClient) { }
    public get Http() {
        return this.http;
    }

    private bearerTokenFactory: (() => string) | undefined;
    public set bearerToken(bearerTokenFactory: () => string) {
        this.bearerTokenFactory = bearerTokenFactory;
    }

    public get<T>(url: string, options = {}): Observable<T> {
        this.handleOptions(options);
        return this.http.get<T>(url, options);
    }
    public getText<T>(url: string, options = {}): Observable<T> {
        this.handleOptions(options);
        (<any>options).responseType = "text";
        return this.http.get<T>(url, options);
    }

    public post<T>(url: string, data: any, options = {}): Observable<T> {
        this.handleOptions(options);
        return this.http.post<T>(url, data, options);
    }
    public postFormData<T>(url: string, formData: FormData): Observable<T> {
        const options = {};
        this.handleOptions(options);
        return this.http.post<T>(url, formData, options);
    }

    public postText<T>(url: string, data: any, options = {}): Observable<T> {
        this.handleOptions(options);
        (<any>options).responseType = "text";
        return this.http.post<T>(url, data, options);
    }

    private handleOptions(options: any) {
        if (this.bearerTokenFactory) {
            if (!options.headers) options.headers = new HttpHeaders();
            options.headers = options.headers.append("Authorization", `Bearer ${this.bearerTokenFactory()}`);
        }
    }
}
