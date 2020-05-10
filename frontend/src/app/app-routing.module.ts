import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {PriceUpdateComponent} from "./components/BP1/price-update/price-update.component";
import {OrderingCancelationComponent} from "./components/BP1/ordering-cancelation/ordering-cancelation.component";
import {LoginComponent} from "./components/login/login.component";
import {AdvertisementPickingComponent} from "./components/BP1/advertisement-picking/advertisement-picking.component";
import {AuthGuard} from "./authGuard";
import {SalesOptimalizationComponent} from "./components/sales-optimalization/sales-optimalization.component";
import {NoTaskComponent} from "./components/no-task/no-task.component";


const routes: Routes = [
  { path: '', component: LoginComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent },
  { path: 'sales-optimalization', component: SalesOptimalizationComponent, canActivate: [AuthGuard],
    children: [
      { path: 'price-update', component: PriceUpdateComponent, canActivate: [AuthGuard]},
      { path: 'ordering-cancellation', component: OrderingCancelationComponent, canActivate: [AuthGuard]},
      { path: 'advertisement-picking', component: AdvertisementPickingComponent, canActivate: [AuthGuard]},
      { path: '**', component: NoTaskComponent, canActivate: [AuthGuard]}

    ] },

  // otherwise redirect to home
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

