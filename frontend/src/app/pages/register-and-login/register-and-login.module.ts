import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {LoginViewComponent} from './login-view/login-view.component';
import {ReactiveFormsModule} from "@angular/forms";
import {RegisterViewComponent} from './register-view/register-view.component';
import {OnboardingComponent} from "./onboarding/onboarding.component";
import {UserDetailsModule} from "../user-details/user-details.module";
import {OnboardingWeightComponent} from "./onboarding-weight/onboarding-weight.component";


@NgModule({
  declarations: [
    LoginViewComponent,
    RegisterViewComponent,
    OnboardingComponent,
    OnboardingWeightComponent
  ],
  exports: [
    LoginViewComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    UserDetailsModule,
  ]
})
export class RegisterAndLoginModule {
}
