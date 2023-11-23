import {Component, EventEmitter, inject, OnInit, Output} from '@angular/core';
import {WeightService} from "../weight.service";
import {WeightDto} from "../weight-dto";

@Component({
  selector: 'app-weights-table',
  templateUrl: './weights-table.component.html',
  styleUrls: ['./weights-table.component.css']
})
export class WeightsTableComponent implements OnInit{
  public readonly weightService = inject(WeightService);
  selectedId = 0;
  constructor() { }

  async ngOnInit() {
    await this.weightService.getWeights();
  }

  async deleteWeight(weight: WeightDto) {
    await this.weightService.deleteWeight(weight);
  }

  setEditingWeight(id: number) {
    this.selectedId = id;
    this.weightService.setEditingWeight(id);
  }
}
