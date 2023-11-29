import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginViewComponent} from "./register-and-login/login-view/login-view.component";
import {RegisterViewComponent} from "./register-and-login/register-view/register-view.component";
import {AuthGuardService} from "./auth-guard.service";
import {HomeViewComponent} from "./home/home-view/home-view.component";
import {NotFoundComponent} from "./not-found/not-found.component";
import {RecordsEditorComponent} from "./pages/records-editor/records-editor.component";
import {AccountDetailsComponent} from "./user-details/account-details/account-details.component";
import {WeightInputComponent} from "./weight-controls/weight-input/weight-input.component";
import {OnboardingComponent} from "./register-and-login/onboarding/onboarding.component";

const routes: Routes = [
    {
        path: "home",
        component: HomeViewComponent,
        canActivate: [AuthGuardService]
    },
    {
        path: "",
        redirectTo: "home",
        pathMatch: "full"
    },
    {
        path: "login",
        component: LoginViewComponent,
    },
    {
        path: "register",
        component: RegisterViewComponent,
    },
    {
        path: "editor",
        component: RecordsEditorComponent,
        canActivate: [AuthGuardService]
    },
    {
      path: "profile",
      component: AccountDetailsComponent,
    },
  {
    path: "input",
    component: WeightInputComponent,
  },
  {
    path:"onboarding",
    component: OnboardingComponent
  },
  {path: '404', component: NotFoundComponent},
  {path: '**', redirectTo: '404'},

];

@NgModule({
    imports: [RouterModule.forRoot(routes, {bindToComponentInputs: true})],
    exports: [RouterModule]
})
export class AppRoutingModule {
}
