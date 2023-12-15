import {inject, Injectable, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {environment} from "../../environments/environment";
import {WeightDto} from "../dtos/weight-dto";

@Injectable({
  providedIn: 'root'
})
export class StatisticsService implements OnInit {
  private readonly http: HttpClient = inject(HttpClient);

  constructor() {
  }

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

  public async getWeightToGo() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/weightToGo");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }

  public async getPercentageOfGoal() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/percentageOfGoal");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }

  async getDaysToGo() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/daysToTarget");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }

  async getDayIn() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/daysIn");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }

  async getPredictedTargetDate() {
    try {
      const call = this.http.get<Date>(environment.baseUrl + "/statistics/predictedTargetDate");
      return await firstValueFrom<Date>(call);
    } catch (e) {
      throw e;
    }
  }

  async getPredictedTargetWeight() {
    try {
      const call = this.http.get<WeightDto>(environment.baseUrl + "/statistics/predictedTargetWeight");
      return await firstValueFrom<WeightDto>(call);
    } catch (e) {
      throw e;
    }
  }

  async getBmiChange() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/bmiChange");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }

  async getAverageDailyLoss() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/averageLoss");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }

  async getAverageWeeklyLoss() {
    try {
      const call = this.http.get<number>(environment.baseUrl + "/statistics/averageLossPerWeek");
      return await firstValueFrom<number>(call);
    } catch (e) {
      throw e;
    }
  }

  async getLowestWeight() {
    try {
      const call = this.http.get<WeightDto>(environment.baseUrl + "/statistics/lowestWeight");
      return await firstValueFrom<WeightDto>(call);
    } catch (e) {
      throw e;
    }
  }
}
