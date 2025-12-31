import { Component } from '@angular/core';
import { FooterLinksService } from './footer-links.service';

@Component({
  selector: 'lpx-footer',
  templateUrl: './footer.component.html',
})
export class FooterComponent {
  footerValues$ = this.service.footerInfo$;
 
  constructor(private service: FooterLinksService) {}
}
