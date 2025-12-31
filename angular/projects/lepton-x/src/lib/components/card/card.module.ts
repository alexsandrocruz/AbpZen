import { NgModule } from '@angular/core';
import { CardComponent } from './card.component';

import { CommonModule } from '@angular/common';
import { CardFooterComponent } from './components/card-footer.component';
import { CardHeaderComponent } from './components/card-header.component';

const declarationsWithExports = [
  CardComponent,
  CardFooterComponent,
  CardHeaderComponent,
];

@NgModule({
  declarations: [...declarationsWithExports],
  imports: [CommonModule],
  exports: [...declarationsWithExports],
})
export class CardModule {}
