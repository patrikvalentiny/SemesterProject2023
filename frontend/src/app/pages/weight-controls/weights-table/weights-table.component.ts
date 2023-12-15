import {Component, inject, OnInit} from '@angular/core';
import {WeightService} from "../../../services/weight.service";
import {WeightDto} from "../../../dtos/weight-dto";

@Component({
  selector: 'app-weights-table',
  templateUrl: './weights-table.component.html',
  styleUrls: ['./weights-table.component.css']
})
export class WeightsTableComponent implements OnInit {
  public readonly weightService = inject(WeightService);
  public selectedDate: Date | null = null;
  days: string[] = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];


  constructor() {
  }

  async ngOnInit() {
    await this.weightService.getWeights();
  }

  setEditingWeight(date: Date) {
    this.selectedDate = date;
    this.weightService.setEditingWeight(date);
  }

  setColStart(weight: WeightDto) {
    return "col-start-" + (this.days.indexOf(new Date(weight.date).toLocaleDateString("en-US", {weekday: "short"})) + 1);
  }

  protected readonly Array = Array;
  protected readonly length = length;
}
