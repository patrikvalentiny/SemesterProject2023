import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {AccountDetailsComponent} from "./account-details/account-details.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";


@NgModule({
  declarations: [
    AccountDetailsComponent,
  ],
  exports: [
    AccountDetailsComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class UserDetailsModule {
}
