import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment.development";
import {firstValueFrom} from "rxjs";
import {User} from "../user";
import {TokenService} from "../token.service";

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  httpClient: HttpClient = inject(HttpClient);
  tokenService:TokenService = inject(TokenService)
  constructor() { }

  async login(username:string, password:string){
    const call = this.httpClient.post<{token:string, user:User}>(environment.baseUrl + "/account/login",  { username: username, password:password });
    const response = await firstValueFrom<{token:string, user:User}>(call);
    this.tokenService.setToken(response.token);

  }
}
