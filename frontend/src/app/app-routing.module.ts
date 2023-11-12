import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginViewComponent} from "./register-and-login/login-view/login-view.component";
import {RegisterViewComponent} from "./register-and-login/register-view/register-view.component";
import {HomeViewComponent} from "./home/home-view/home-view.component";

const routes: Routes = [
  {
    path:"home",
    component: HomeViewComponent
  },
  {
    path:"",
    redirectTo:"home",
    pathMatch:"full"
  },
  {
    path:"login",
    component: LoginViewComponent,
  },
  {
    path:"register",
    component: RegisterViewComponent,
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {bindToComponentInputs:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
