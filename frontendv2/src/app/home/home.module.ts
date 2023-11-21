import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeSkeletonComponent } from './home-skeleton/home-skeleton.component';
import {RouterOutlet} from "@angular/router";
import { HomeViewComponent } from './home-view/home-view.component';
import { WeightControlsModule } from '../weight-controls/weight-controls.module';



@NgModule({
  declarations: [
    HomeSkeletonComponent,
    HomeViewComponent
  ],
  exports: [
    HomeSkeletonComponent
  ],
    imports: [
        CommonModule,
        RouterOutlet,
        WeightControlsModule
    ]
})
export class HomeModule { }
