import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpResponse} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class StatusService {
  private readonly http = inject(HttpClient);
  constructor() { }

  async isRunning() {
    try {
      const call = this.http.get<string>(environment.baseUrl + "/status", {observe: "response", responseType:'text' as 'json'});
      const response = await firstValueFrom<HttpResponse<string>>(call);
    } catch (e) {
      return;
    }
  }
}
