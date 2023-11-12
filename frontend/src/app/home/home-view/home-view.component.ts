import {Component, inject, OnInit} from '@angular/core';
import {User} from "../../user";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../../environments/environment.development";
import {firstValueFrom} from "rxjs";

@Component({
  selector: 'app-home-view',
  templateUrl: './home-view.component.html',
  styleUrls: ['./home-view.component.css']
})
export class HomeViewComponent implements OnInit{
  private readonly httpClient: HttpClient = inject(HttpClient);


  public  user: User | null = null;
  async ngOnInit() {
    this.user = await firstValueFrom<User>(this.httpClient.get<User>(environment.baseUrl + "/account/whoami"));
  }

}
