import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginViewComponent} from "./register-and-login/login-view/login-view.component";

const routes: Routes = [
  {
    path:"home",
    component: LoginViewComponent
  },
  {
    path:"",
    redirectTo:"home",
    pathMatch:"full"
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {bindToComponentInputs:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
