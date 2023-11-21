import {NgModule} from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {LoginViewComponent} from "./register-and-login/login-view/login-view.component";
import {RegisterViewComponent} from "./register-and-login/register-view/register-view.component";
import {AuthGuardService} from "./auth-guard.service";
import {HomeViewComponent} from "./home/home-view/home-view.component";
import {NotFoundComponent} from "./not-found/not-found.component";
import {RecordsEditorComponent} from "./pages/records-editor/records-editor.component";

const routes: Routes = [
  {
    path:"home",
    component: HomeViewComponent,
    canActivate: [AuthGuardService]
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
  },
  {
    path:"editor",
    component:RecordsEditorComponent,
    canActivate: [AuthGuardService]
  },
  {path: '404', component: NotFoundComponent},
  {path: '**', redirectTo: '404'},

];

@NgModule({
  imports: [RouterModule.forRoot(routes, {bindToComponentInputs:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
