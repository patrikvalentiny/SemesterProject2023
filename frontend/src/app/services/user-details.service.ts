import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {UserDetails} from "../dtos/user-details";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";
import {HotToastService} from "@ngneat/hot-toast";
import {FormGroup} from "@angular/forms";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class UserDetailsService {
  private readonly httpClient = inject(HttpClient);
  private readonly toastService = inject(HotToastService);
  private readonly router = inject(Router);
  user:UserDetails | null = null;
  async getProfile() {
    try {
      const call = this.httpClient.get<UserDetails>(environment.baseUrl + "/profile");
      const user = await firstValueFrom<UserDetails>(call);
      this.user = user;
      return user;
    } catch (e) {
      this.toastService.error("Failed to get profile")
    }
    return null;
  }

  async createProfile(formGroup : FormGroup) {
    try {
      const call = this.httpClient.post<UserDetails>(environment.baseUrl + "/profile", formGroup.value);
      this.user = await firstValueFrom<UserDetails>(call);
      this.toastService.success("Profile created")
      await this.router.navigate(["/home"])
    } catch (e) {
      this.toastService.error("Failed to create profile")
    }
  }
  async updateProfile(formGroup : FormGroup) {
    if (this.user == null) {
      return await this.createProfile(formGroup)
    }
    try {
      const call = this.httpClient.put<UserDetails>(environment.baseUrl + "/profile", formGroup.value);
      this.user = await firstValueFrom<UserDetails>(call);
      this.toastService.success("Profile updated")
    } catch (e) {
      this.toastService.error("Failed to update profile")
    }

  }
}
