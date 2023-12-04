import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginViewComponent} from "./pages/register-and-login/login-view/login-view.component";
import {RegisterViewComponent} from "./pages/register-and-login/register-view/register-view.component";
import {AuthGuardService} from "./auth-guard.service";
import {HomeViewComponent} from "./home/home-view/home-view.component";
import {NotFoundComponent} from "./pages/not-found/not-found.component";
import {RecordsEditorComponent} from "./pages/records-editor/records-editor.component";
import {AccountDetailsComponent} from "./pages/user-details/account-details/account-details.component";
import {WeightInputComponent} from "./pages/weight-controls/weight-input/weight-input.component";
import {OnboardingComponent} from "./pages/register-and-login/onboarding/onboarding.component";
import {SignupGuardService} from "./signup-guard.service";
import {OnboardingWeightComponent} from "./pages/register-and-login/onboarding-weight/onboarding-weight.component";

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
        canActivate: [SignupGuardService]
    },
    {
        path: "register",
        component: RegisterViewComponent,
      canActivate: [SignupGuardService]
    },
    {
        path: "editor",
        component: RecordsEditorComponent,
        canActivate: [AuthGuardService]
    },
    {
      path: "profile",
      component: AccountDetailsComponent,
      canActivate: [AuthGuardService]
    },
  {
    path: "input",
    component: WeightInputComponent,
    canActivate: [AuthGuardService]
  },
  {
    path:"onboarding/profile",
    component: OnboardingComponent
  },
  {
    path:"onboarding/weight",
    component: OnboardingWeightComponent
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
