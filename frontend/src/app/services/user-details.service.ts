import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {UserDetails} from "../dtos/user-details";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";
import {FormGroup} from "@angular/forms";
import {Router} from "@angular/router";
import {User} from "../dtos/user";
import {TokenService} from "./token.service";

@Injectable({
  providedIn: 'root'
})
export class UserDetailsService {
  user: UserDetails | null = null;
  private readonly httpClient = inject(HttpClient);
  private readonly router = inject(Router);
  private readonly tokenService = inject(TokenService);

  async getProfile() {
    try {
      const call = this.httpClient.get<UserDetails>(environment.baseUrl + "/profile");
      const user = await firstValueFrom<UserDetails>(call);
      this.user = user;
      return user;
    } catch (e) {
      throw e;
    }
  }

  async createProfile(formGroup: FormGroup) {
    try {
      const call = this.httpClient.post<UserDetails>(environment.baseUrl + "/profile", formGroup.value);
      this.user = await firstValueFrom<UserDetails>(call);
      await this.router.navigate(["/home"])
    } catch (e) {
      throw e;
    }
  }

  async updateProfile(formGroup: FormGroup) {
    if (this.user == null) {
      return await this.createProfile(formGroup)
    }
    try {
      const call = this.httpClient.put<UserDetails>(environment.baseUrl + "/profile", formGroup.value);
      this.user = await firstValueFrom<UserDetails>(call);
    } catch (e) {
      throw e;
    }

  }

  async deleteProfile() {
    try {
      const call = this.httpClient.delete<User>(environment.baseUrl + "/account");
      await firstValueFrom<User>(call);
      this.user = null;
      this.tokenService.clearToken();
      await this.router.navigate(["/login"])
    } catch (e) {
      throw e;
    }
  }
}
