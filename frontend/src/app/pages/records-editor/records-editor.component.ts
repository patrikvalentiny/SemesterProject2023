import {Component} from '@angular/core';
import { EditWeightComponent } from '../weight-controls/edit-weight/edit-weight.component';
import { WeightsTableComponent } from '../weight-controls/weights-table/weights-table.component';

@Component({
    selector: 'app-records-editor',
    host: { class: 'h-full' },
    templateUrl: './records-editor.component.html',
    styleUrls: ['./records-editor.component.css'],
    standalone: true,
    imports: [WeightsTableComponent, EditWeightComponent]
})
export class RecordsEditorComponent {

}
