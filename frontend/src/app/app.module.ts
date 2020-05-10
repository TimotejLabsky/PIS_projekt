import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { LoginComponent } from './components/login/login.component';
import {MatCardModule} from "@angular/material/card";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatProgressSpinnerModule} from "@angular/material/progress-spinner";
import {ReactiveFormsModule, FormsModule} from "@angular/forms";
import { MatInputModule} from '@angular/material/input';
import {MatButtonModule} from "@angular/material/button";
import {MatIconModule} from "@angular/material/icon";
import {MatToolbarModule} from "@angular/material/toolbar";
import { PriceUpdateComponent } from './components/price-update/price-update.component';
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatTableModule} from "@angular/material/table";
import { AdvertisementPickingComponent } from './components/advertisement-picking/advertisement-picking.component';
import {MatCheckboxModule} from "@angular/material/checkbox";
import { OrderingCancelationComponent } from './components/ordering-cancelation/ordering-cancelation.component';
import { SalesOptimalizationComponent } from './components/sales-optimalization/sales-optimalization.component';
import {RouterModule} from "@angular/router";
import { NoTaskComponent } from './components/no-task/no-task.component';
import { IncludeToSeasonComponent } from './components/include-to-season/include-to-season.component';
import { HttpClientModule } from '@angular/common/http'

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    PriceUpdateComponent,
    AdvertisementPickingComponent,
    OrderingCancelationComponent,
    SalesOptimalizationComponent,
    NoTaskComponent,
    IncludeToSeasonComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MatCardModule,
    MatFormFieldModule,
    MatProgressSpinnerModule,
    ReactiveFormsModule,
    FormsModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatPaginatorModule,
    MatTableModule,
    MatCheckboxModule,
    HttpClientModule
  ],
  providers: [ ],
  bootstrap: [AppComponent]
})
export class AppModule { }
