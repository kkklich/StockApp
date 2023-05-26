import { Component } from '@angular/core';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent {

  public companyArray: string[] = ["PKN ORLEN", "XTB"];
  public selectedMedia: any;

  constructor() { }

  changeCompany() {
    console.log(this.selectedMedia)
  }

}
