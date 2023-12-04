import {Component, inject, OnInit} from '@angular/core';
import {StatisticsService} from "../../services/statistics.service";

@Component({
    selector: 'app-home-view',
    host: {class: 'h-full'},
    templateUrl: './home-view.component.html',
    styleUrls: ['./home-view.component.css']
})
export class HomeViewComponent implements OnInit{

  constructor() {

  }

  async ngOnInit() {

  }
}
