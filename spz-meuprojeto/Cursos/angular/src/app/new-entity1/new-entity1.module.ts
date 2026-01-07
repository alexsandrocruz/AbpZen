import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';
import { NewEntity1RoutingModule } from './new-entity1-routing.module';
import { NewEntity1Component } from './new-entity1.component';

@NgModule({
  declarations: [NewEntity1Component],
  imports: [
    SharedModule,
    NewEntity1RoutingModule,
    PageModule,
  ],
})
export class NewEntity1Module {}
