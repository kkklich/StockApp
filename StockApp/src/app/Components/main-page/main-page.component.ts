import { Component } from '@angular/core';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {

  public companyArray: any[] = [{ name: "PKN ORLEN", symbol: "PKN" }, { name: "XTB", symbol: "XTB" }];
  public selectedMedia: any;

  constructor() {
    this.selectedMedia = this.companyArray[0];
  }

  changeCompany() {
  }

}
