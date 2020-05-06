import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {PriceUpdateComponent} from "./components/price-update/price-update.component";
import {OrderingCancelationComponent} from "./components/ordering-cancelation/ordering-cancelation.component";
import {LoginComponent} from "./components/login/login.component";
import {AdvertisementPickingComponent} from "./components/advertisement-picking/advertisement-picking.component";
import {AuthGuard} from "./authGuard";
import {HomeComponent} from "./components/home/home.component";


const routes: Routes = [
  { path: '', component: LoginComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'home', component: HomeComponent, canActivate: [AuthGuard],
    children: [
      {path: '', component: PriceUpdateComponent, canActivate: [AuthGuard]},
      { path: 'price-update', component: PriceUpdateComponent, canActivate: [AuthGuard]},
      { path: 'ordering-cancellation', component: OrderingCancelationComponent, canActivate: [AuthGuard]},
      { path: 'advertisement-picking', component: AdvertisementPickingComponent, canActivate: [AuthGuard]},
    ] },

  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }


/*

* */
