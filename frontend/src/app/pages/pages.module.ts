import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RecordsEditorComponent} from './records-editor/records-editor.component';
import {WeightControlsModule} from "../weight-controls/weight-controls.module";


@NgModule({
    declarations: [
        RecordsEditorComponent,
    ],
    exports: [],
    imports: [
        CommonModule,
        WeightControlsModule
    ]
})
export class PagesModule {
}
