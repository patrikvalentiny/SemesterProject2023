import {NgModule, isDevMode} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {RegisterAndLoginModule} from "./register-and-login/register-and-login.module";
import {HTTP_INTERCEPTORS, HttpClientModule} from "@angular/common/http";
import {AuthHttpInterceptor} from "./interceptors/auth-http-interceptor";
import {TokenService} from "./token.service";
import {provideHotToastConfig} from '@ngneat/hot-toast';
import {ErrorHttpInterceptor} from "./interceptors/error-http-interceptor";
import {HomeModule} from "./home/home.module";
import {NotFoundComponent} from "./not-found/not-found.component";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";
import {PagesModule} from "./pages/pages.module";
import {NgApexchartsModule} from "ng-apexcharts";
import { ServiceWorkerModule } from '@angular/service-worker';

@NgModule({
    declarations: [
        AppComponent,
        NotFoundComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        RegisterAndLoginModule,
        HttpClientModule,
        HomeModule,
        ReactiveFormsModule,
        FormsModule,
        PagesModule,
        NgApexchartsModule,
        ServiceWorkerModule.register('ngsw-worker.js', {
          enabled: !isDevMode(),
          // Register the ServiceWorker as soon as the application is stable
          // or after 30 seconds (whichever comes first).
          registrationStrategy: 'registerWhenStable:30000'
        })
    ],
    providers: [TokenService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: AuthHttpInterceptor,
            multi: true
        },
        provideHotToastConfig(
            {
                position: 'bottom-center',
                theme: "snackbar"
            }),
        {
            provide: HTTP_INTERCEPTORS,
            useClass: ErrorHttpInterceptor,
            multi: true
        }
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
