import {inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {firstValueFrom} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class CsvService {
  private readonly http = inject(HttpClient);

  constructor() {
  }

  async uploadCSV(file: File) {
    try {
      const formData = new FormData();
      formData.append('file', file);
      const call = this.http.post<null>(environment.baseUrl + "/csv", formData);
      const response = await firstValueFrom<null>(call);
    } catch (e) {
      return;
    }
  }
}
