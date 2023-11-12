import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {RegisterAndLoginModule} from "./register-and-login/register-and-login.module";

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RegisterAndLoginModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
