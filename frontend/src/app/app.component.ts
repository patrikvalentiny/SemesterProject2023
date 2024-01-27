import {Component} from '@angular/core';
import { HomeSkeletonComponent } from './home/home-skeleton/home-skeleton.component';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    standalone: true,
    imports: [HomeSkeletonComponent]
})
export class AppComponent {
  title = 'WeightTracker';
}
