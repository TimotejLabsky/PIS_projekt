import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {PriceUpdateComponent} from "./components/price-update/price-update.component";
import {OrderingCancelationComponent} from "./components/ordering-cancelation/ordering-cancelation.component";
import {LoginComponent} from "./components/login/login.component";
import {AdvertisementPickingComponent} from "./components/advertisement-picking/advertisement-picking.component";


const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full'},
  { path: 'login', component: LoginComponent},
  { path: 'price-update', component: PriceUpdateComponent},
  { path: 'ordering-cancellation', component: OrderingCancelationComponent},
  { path: 'advertisement-picking', component: AdvertisementPickingComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
