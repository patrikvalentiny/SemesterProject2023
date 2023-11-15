import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {TokenService} from "../token.service";
import {Router} from "@angular/router";
import {firstValueFrom} from "rxjs";
import {User} from "../user";
import {environment} from "../../environments/environment.development";

@Injectable({
  providedIn: 'root'
})
export class WeightService {

  httpClient: HttpClient = inject(HttpClient);
  tokenService:TokenService = inject(TokenService);
  router:Router = inject(Router);
  constructor() { }

  async postWeight(value: number) {
    try {
      const call = this.httpClient.post(environment.baseUrl + "/weight",  { weight: value, date: new Date()});
      const response = await firstValueFrom(call);
    } catch (e) {

    }
  }
}
