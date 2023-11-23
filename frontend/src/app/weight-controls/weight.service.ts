import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {firstValueFrom, Subject} from "rxjs";
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
  editingWeight = new Subject<WeightDto>();
  constructor() { }

  async postWeight(value: number, date:string, time:string = "00:00") {
    try {
      const call = this.httpClient.post<WeightDto>(environment.baseUrl + "/weight",  { weight: value, date: new Date(`${date}T${time}:00`)});
      const response = await firstValueFrom<WeightDto>(call);

      this.toastService.success("Weight successfully added")
      this.weights.unshift(response)

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

  async deleteWeight(weight: WeightDto) {
    try {
    const call = this.httpClient.delete<WeightDto>(environment.baseUrl + `/weight/${weight.id}`);
    await firstValueFrom<WeightDto>(call);
    this.weights = this.weights.filter(i => i.id !== weight.id)
    this.toastService.success("Item deleted")
    } catch (e) {

    }
  }

    async getLatestWeight() {
        try {
            const call = this.httpClient.get<WeightDto>(environment.baseUrl + "/weight/latest");
            return await firstValueFrom<WeightDto>(call);
        } catch (e) {
            return;
        }
    }

  async putWeight(id: number, weight:number, date: string, time: string = "00:00") {
    try {
      const call = this.httpClient.put<WeightDto>(environment.baseUrl + "/weight",  { id: id, weight: weight, date: new Date(`${date}T${time}:00`)});
      const response = await firstValueFrom<WeightDto>(call);

      this.toastService.success("Weight successfully updated")
      await this.getWeights();

    } catch (e) {

    }
  }

  setEditingWeight(id: number) {
    this.editingWeight.next(this.weights.find(i => i.id === id)!);
  }
}
