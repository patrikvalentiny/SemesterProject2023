import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeSkeletonComponent } from './home-skeleton/home-skeleton.component';
import {RouterOutlet} from "@angular/router";
import { HomeViewComponent } from './home-view/home-view.component';
import {WeightInputModule} from "../weight-input/weight-input.module";



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
        WeightInputModule
    ]
})
export class HomeModule { }
