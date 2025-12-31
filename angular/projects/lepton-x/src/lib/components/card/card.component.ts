import { Component, ContentChild } from '@angular/core';
import { CardFooterComponent } from './components/card-footer.component';
import { CardHeaderComponent } from './components/card-header.component';

@Component({
  selector: 'lpx-card',
  templateUrl: './card.component.html',
})
export class CardComponent {
  @ContentChild(CardFooterComponent)
  cardFooterTemplate?: CardFooterComponent;

  @ContentChild(CardHeaderComponent)
  cardHeaderTemplate?: CardHeaderComponent;
}
