import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MainPageComponent } from './Components/main-page/main-page.component';
import { FormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoadCSVService } from './Services/load-csv.service';
import { HttpClientModule } from '@angular/common/http';
import { ChartComponent } from './Components/chart/chart.component';
import { PatternService } from './Services/pattern.service';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { ApiHttpService } from './Services/api-http.service';
import { ChartService } from './Services/chart.service';

@NgModule({
  declarations: [
    AppComponent,
    MainPageComponent,
    ChartComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    MatSelectModule,
    BrowserAnimationsModule,
    HttpClientModule,
    MatIconModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule
  ],
  providers: [
    LoadCSVService,
    PatternService,
    ApiHttpService,
    ChartService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
