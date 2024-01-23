import {Component, inject} from '@angular/core';
import {CsvService} from "../../services/csv.service";
import {HotToastService} from "@ngneat/hot-toast";

@Component({
    selector: 'app-csv-controls',
    templateUrl: './csv-controls.component.html',
    styleUrl: './csv-controls.component.css',
    standalone: true
})
export class CsvControlsComponent {
  file: File | null = null;
  private readonly csvService = inject(CsvService);
  private readonly toast = inject(HotToastService);

  async uploadCSV() {
    if (!this.file) {
      this.toast.error("No file selected");
      return;
    }
    await this.csvService.uploadCSV(this.file);
  }

  onFileChange(event: Event) {
    this.file = (event.target as HTMLInputElement).files![0];
  }


}
