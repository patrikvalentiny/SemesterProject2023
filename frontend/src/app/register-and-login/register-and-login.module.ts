import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginViewComponent } from './login-view/login-view.component';



@NgModule({
  declarations: [
    LoginViewComponent
  ],
  exports: [
    LoginViewComponent
  ],
  imports: [
    CommonModule
  ]
})
export class RegisterAndLoginModule { }
