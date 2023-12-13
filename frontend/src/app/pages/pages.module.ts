import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RecordsEditorComponent} from './records-editor/records-editor.component';
import {WeightControlsModule} from "./weight-controls/weight-controls.module";
import {CsvControlsComponent} from "./csv-controls/csv-controls.component";
import {ReactiveFormsModule} from "@angular/forms";
import {NotFoundComponent} from "./not-found/not-found.component";


@NgModule({
    declarations: [
      RecordsEditorComponent,
      CsvControlsComponent,
      NotFoundComponent
    ],
  imports: [
    CommonModule,
    WeightControlsModule,
    ReactiveFormsModule
  ]
})
export class PagesModule {
}
