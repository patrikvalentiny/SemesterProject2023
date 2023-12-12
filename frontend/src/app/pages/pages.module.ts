import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RecordsEditorComponent} from './records-editor/records-editor.component';
import {WeightControlsModule} from "./weight-controls/weight-controls.module";
import {AverageLossCardComponent} from "./stat-cards/average-loss-card/average-loss-card.component";
import {CsvControlsComponent} from "./csv-controls/csv-controls.component";
import {ReactiveFormsModule} from "@angular/forms";


@NgModule({
    declarations: [
        RecordsEditorComponent,
      AverageLossCardComponent,
      CsvControlsComponent
    ],
  exports: [
    AverageLossCardComponent
  ],
  imports: [
    CommonModule,
    WeightControlsModule,
    ReactiveFormsModule
  ]
})
export class PagesModule {
}
