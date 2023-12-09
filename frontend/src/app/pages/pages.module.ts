import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RecordsEditorComponent} from './records-editor/records-editor.component';
import {WeightControlsModule} from "./weight-controls/weight-controls.module";
import {AverageLossCardComponent} from "./stat-cards/average-loss-card/average-loss-card.component";


@NgModule({
    declarations: [
        RecordsEditorComponent,
      AverageLossCardComponent
    ],
  exports: [
    AverageLossCardComponent
  ],
    imports: [
        CommonModule,
        WeightControlsModule
    ]
})
export class PagesModule {
}
