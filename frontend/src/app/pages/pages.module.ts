import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RecordsEditorComponent} from './records-editor/records-editor.component';
import {WeightControlsModule} from "./weight-controls/weight-controls.module";
import {CsvControlsComponent} from "./csv-controls/csv-controls.component";
import {ReactiveFormsModule} from "@angular/forms";
import {NotFoundComponent} from "./not-found/not-found.component";
import {PasteDataFromExcelComponent} from "./paste-data-from-excel/paste-data-from-excel.component";


@NgModule({
  declarations: [
    RecordsEditorComponent,
    CsvControlsComponent,
    NotFoundComponent,
    PasteDataFromExcelComponent
  ],
  imports: [
    CommonModule,
    WeightControlsModule,
    ReactiveFormsModule
  ]
})
export class PagesModule {
}
