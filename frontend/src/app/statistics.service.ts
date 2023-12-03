import {inject, Injectable, OnInit} from '@angular/core';
import {WeightDto} from "./weight-controls/weight-dto";
import {HttpClient} from "@angular/common/http";
import {environment} from "../environments/environment";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class StatisticsService implements OnInit{
  http: HttpClient = inject(HttpClient);

  constructor() { }

  async ngOnInit() {
  }

  public async getTrend() {
    try {
      const call = this.http.get<WeightDto[]>(environment.baseUrl + "/statistics/currentTrend");
      return await firstValueFrom<WeightDto[]>(call);
    } catch (e) {
      throw e;
    }
  }

  public async getCurrentTotalLoss() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/currentTotalLoss");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }

  public async getWeighToGo() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/weighToGo");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }

  public async getPercentageOfGoal() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/getPercentageOfGoal");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }
}
