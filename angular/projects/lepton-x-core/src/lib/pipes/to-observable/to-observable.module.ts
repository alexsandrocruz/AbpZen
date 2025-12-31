import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToObservablePipe } from './to-observable.pipe';

@NgModule({
  declarations: [ToObservablePipe],
  imports: [CommonModule],
  exports: [ToObservablePipe],
})
export class ToObservableModule {}
