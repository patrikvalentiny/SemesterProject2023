import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom, Subject} from "rxjs";
import {environment} from "../../environments/environment";
import {HotToastService} from "@ngneat/hot-toast";
import {WeightDto} from "../dtos/weight-dto";
import {Bmi} from "../dtos/bmi";
import {WeightInput} from "../dtos/weight-input";

@Injectable({
  providedIn: 'root'
})
export class WeightService {
  weights: WeightDto[] = [];
  editingWeight = new Subject<WeightDto>();
  private readonly toastService = inject(HotToastService);
  private readonly httpClient: HttpClient = inject(HttpClient);

  constructor() {
  }

  async postWeight(weight:WeightInput) {
    try {
      const call = this.httpClient.post<WeightDto>(environment.baseUrl + "/weight", weight);
      const response = await firstValueFrom<WeightDto>(call);

      this.toastService.success("Weight successfully added");
      this.weights.unshift(response);
    } catch (e) {
      return;
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

  async deleteWeight(weight: WeightInput) {
    try {
      const date = weight.date.toISOString().substring(0, 10);
      const call = this.httpClient.delete<WeightDto>(environment.baseUrl + `/weight/${date}`);
      await firstValueFrom<WeightDto>(call);
      await this.getWeights();
      this.toastService.success("Item deleted");
    } catch (e) {

    }
  }

  async getLatestWeight() {
    try {
      if (this.weights.length === 0) await this.getWeights();
      return this.weights[this.weights.length - 1];
    } catch (e) {
      return;
    }
  }

  async putWeight(weightInput:WeightInput) {
    try {
      const call = this.httpClient.put<WeightDto>(environment.baseUrl + "/weight", weightInput);
      const response = await firstValueFrom<WeightDto>(call);
      await this.getWeights();
      this.toastService.success("Weight successfully updated");
    } catch (e) {

    }
  }

  setEditingWeight(date: Date) {
    this.editingWeight.next(this.weights.find(i => i.date === date)!);
  }

  async getLatestBmi() {
    try {
      const call = this.httpClient.get<Bmi>(environment.baseUrl + "/bmi/latest");
      return await firstValueFrom<Bmi>(call);
    } catch (e) {
      return;
    }
  }

  async getBmi() {
    try {
      const call = this.httpClient.get<Bmi[]>(environment.baseUrl + "/bmi");
      return await firstValueFrom<Bmi[]>(call);
    } catch (e) {
      return;
    }
  }
}
