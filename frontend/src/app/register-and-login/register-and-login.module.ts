import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginViewComponent } from './login-view/login-view.component';
import {ReactiveFormsModule} from "@angular/forms";
import { RegisterViewComponent } from './register-view/register-view.component';


@NgModule({
  declarations: [
    LoginViewComponent,
    RegisterViewComponent
  ],
  exports: [
    LoginViewComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
  ]
})
export class RegisterAndLoginModule { }
