import {Component, inject} from '@angular/core';
import {FormControl} from "@angular/forms";
import {WeightDto} from "../../dtos/weight-dto";
import {WeightService} from "../../services/weight.service";
import {HotToastService} from "@ngneat/hot-toast";
import {Router} from "@angular/router";

@Component({
  selector: 'app-paste-data-from-excel',
  host: {class: 'h-full'},
  templateUrl: './paste-data-from-excel.component.html',
  styleUrl: './paste-data-from-excel.component.css'
})
export class PasteDataFromExcelComponent {
  private readonly weightService = inject(WeightService);
  private readonly toastService = inject(HotToastService);
  private readonly router = inject(Router);
  dataInput:FormControl<string | null> = new FormControl(null);
  weights:WeightDto[] = [];
  constructor() {
  }

  async parseData(){
    this.weights = [];
    const data = this.dataInput.value!;
    const lines = data.split("\n");
    for(let i = 0; i < lines.length; i++){
      const line = lines[i];
      const cells = line.split("\t");
      const weight:WeightDto = {
        weight: Number(cells[1]),
        date: new Date(cells[0])
      };
      weight.date.setUTCFullYear(weight.date.getFullYear(), weight.date.getMonth(), weight.date.getDate());
      weight.date.setUTCHours(0,0,0,0);
      this.weights.push(weight);
    }
    console.log(this.weights)
  }

  async uploadData(){
    try {
      await this.weightService.postMulti(this.weights);
      this.toastService.success("Data successfully uploaded");
      await this.router.navigate(["/home"]);
    } catch (e) {
      return;
    }

  }
}
