import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {environment} from "../../environments/environment";
import {HotToastService} from "@ngneat/hot-toast";
import {WeightDto} from "./weight-dto";

@Injectable({
  providedIn: 'root'
})
export class WeightService {
  private readonly toastService = inject(HotToastService);
  private readonly httpClient: HttpClient = inject(HttpClient);
  weights: WeightDto[] = [];
  constructor() { }

  async postWeight(value: number) {
    try {
      const call = this.httpClient.post<WeightDto>(environment.baseUrl + "/weight",  { weight: value, date: new Date()});
      const response = await firstValueFrom<WeightDto>(call);

      if (call.subscribe()) {
        this.toastService.success("Weight successfully added")
        this.weights.push(response)
      }
    } catch (e) {

    }
  }

  async getWeights() {
    try {
      const call = this.httpClient.get<WeightDto[]>(environment.baseUrl + "/weight");
      this.weights = await firstValueFrom<WeightDto[]>(call);
      return this.weights;
    } catch (e) {
      return;
    }
  }
}
