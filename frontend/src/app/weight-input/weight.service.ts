import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {environment} from "../../environments/environment";
import {HotToastService} from "@ngneat/hot-toast";

@Injectable({
  providedIn: 'root'
})
export class WeightService {
  private readonly toastService = inject(HotToastService);
  private readonly httpClient: HttpClient = inject(HttpClient);

  constructor() { }

  async postWeight(value: number) {
    try {
      const call = this.httpClient.post(environment.baseUrl + "/weight",  { weight: value, date: new Date()});
      const response = await firstValueFrom(call);
      if (call.subscribe()) {
        this.toastService.success("Weight successfully added")
      }
    } catch (e) {

    }
  }
}
