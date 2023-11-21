import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouteReuseStrategy } from '@angular/router';

import { IonicModule, IonicRouteStrategy } from '@ionic/angular';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import {NotFoundComponent} from "./not-found/not-found.component";
import {RegisterAndLoginModule} from "./register-and-login/register-and-login.module";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {HomeModule} from "./home/home.module";
import {TokenService} from "./token.service";
import {AuthHttpInterceptor} from "./interceptors/auth-http-interceptor";
import {provideHotToastConfig} from "@ngneat/hot-toast";
import {ErrorHttpInterceptor} from "./interceptors/error-http-interceptor";

@NgModule({
  declarations: [AppComponent, NotFoundComponent],
  imports: [BrowserModule, IonicModule.forRoot(), AppRoutingModule, RegisterAndLoginModule, HttpClientModule, HomeModule],
  providers: [{ provide: RouteReuseStrategy, useClass: IonicRouteStrategy }, TokenService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true
    },
    provideHotToastConfig(
      {position: 'bottom-center',
        theme: "snackbar"
      }),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ErrorHttpInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent],
})
export class AppModule {}
