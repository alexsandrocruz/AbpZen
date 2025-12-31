import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LpxIconModule } from '../icon/icon.module';
import { AvatarComponent } from './avatar.component';

@NgModule({
  declarations: [AvatarComponent],
  imports: [CommonModule, LpxIconModule],
  exports: [AvatarComponent],
})
export class LpxAvatarModule {}
