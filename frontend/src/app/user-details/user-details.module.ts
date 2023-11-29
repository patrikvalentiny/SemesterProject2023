import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {AccountDetailsComponent} from "./account-details/account-details.component";
import {ReactiveFormsModule} from "@angular/forms";



@NgModule({
  declarations: [
    AccountDetailsComponent,
  ],
  exports: [
    AccountDetailsComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule
  ]
})
export class UserDetailsModule { }
