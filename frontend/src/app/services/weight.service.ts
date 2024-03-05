import {inject, Injectable, OnInit, signal} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom} from "rxjs";
import {environment} from "../../environments/environment";
import {HotToastService} from "@ngneat/hot-toast";
import {WeightDto} from "../dtos/weight-dto";
import {Bmi} from "../dtos/bmi";
import {WeightInput} from "../dtos/weight-input";
import * as FileSaver from "file-saver-es";

@Injectable({
  providedIn: 'root'
})
export class WeightService implements OnInit {
  weights: WeightDto[] = [];
  editingWeight = signal<WeightDto | null>(null);
  private readonly toastService = inject(HotToastService);
  private readonly httpClient: HttpClient = inject(HttpClient);

  constructor() {
  }

  async ngOnInit() {
    await this.getWeights();
  }

  async postWeight(weight: WeightInput) {
    try {
      const call = this.httpClient.post<WeightDto>(environment.baseUrl + "/weight", weight);
      const response = await firstValueFrom<WeightDto>(call);
      this.toastService.success("Weight successfully added");
      let latest = await this.getLatestWeight();
      if (latest!.date === weight.date) {
        await this.putWeight(weight);
      } else {
        this.weights.push(response);
      }
    } catch (e) {
      return;
    }
  }

  async getWeights() {
    try {
      if (this.weights.length > 0) return this.weights;
      const call = this.httpClient.get<WeightDto[]>(environment.baseUrl + "/weight");
      this.weights = await firstValueFrom<WeightDto[]>(call);
      return this.weights;
    } catch (e) {
      return;
    }
  }

  async deleteWeight(date: Date) {
    try {
      const d = date.toISOString().substring(0, 19);
      const call = this.httpClient.delete<WeightDto>(environment.baseUrl + `/weight/${d}`);
      await firstValueFrom<WeightDto>(call);
      const index = this.weights.findIndex(i => i.date.toString() === d);
      this.weights.splice(index, 1)
      this.toastService.success("Weight successfully deleted");
    } catch (e) {
      return;
    }
  }

  async getLatestWeight() {
    try {
      const weights = await this.getWeights();
      return weights!.at(-1)!;
    } catch (e) {
      return;
    }
  }

  async putWeight(weightInput: WeightInput) {
    try {
      const call = this.httpClient.put<WeightDto>(environment.baseUrl + "/weight", weightInput);
      const response = await firstValueFrom<WeightDto>(call);
      console.log(response);
      let index = this.weights.findIndex(i => i.date === weightInput.date);
      this.weights.at(index)!.weight = response.weight;


      this.toastService.success("Weight successfully updated");
    } catch (e) {
      console.log(e);
    }
  }

  setEditingWeight(date: Date) {
    this.editingWeight.set(this.weights.find(i => i.date === date)!);
  }

  async getLatestBmi() {
    try {
      const call = this.httpClient.get<Bmi>(environment.baseUrl + "/bmi/latest");
      return await firstValueFrom<Bmi>(call);
    } catch (e) {
      throw e;
    }
  }

  async getBmi() {
    try {
      const call = this.httpClient.get<Bmi[]>(environment.baseUrl + "/bmi");
      return await firstValueFrom<Bmi[]>(call);
    } catch (e) {
      throw e;
    }
  }

  async postMulti(weights: WeightDto[]) {
    try {
      const call = this.httpClient.post<WeightDto[]>(environment.baseUrl + "/weight/multiple", weights);
      await firstValueFrom<WeightDto[]>(call);
    } catch (e) {
      throw e;
    }
  }

  async getWeightsCsv() {
    try {
      const call = this.httpClient.get(environment.baseUrl + "/csv", {responseType: "blob"});
      const response = await firstValueFrom<Blob>(call);
      const file = new File([response], "weights.csv", {type: "text/csv;charset=utf-8"});
      FileSaver.saveAs(file);
    } catch (e) {
      throw e;
    }
  }
}
