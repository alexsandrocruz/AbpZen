import { Component, computed, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { Subject } from 'rxjs';
import {
  LocalizationModule,
  ReplaceableTemplateDirective,
  RouterEvents,
} from '@abp/ng.core';
import { HTTP_ERROR_STATUS, HTTP_ERROR_DETAIL } from '@abp/ng.theme.shared';
import { eThemeLeptonXComponents } from '../../enums';

@Component({
  standalone: true,
  selector: 'abp-http-error',
  imports: [LocalizationModule, ReplaceableTemplateDirective],
  templateUrl: './http-error.component.html',
})
export class HttpErrorComponent {
  protected readonly router = inject(Router);
  protected readonly activatedRoute = inject(ActivatedRoute);
  protected readonly location = inject(Location);
  protected readonly routerEvents = inject(RouterEvents);

  public readonly destroy$: Subject<void>;

  protected readonly errorComponentKey = eThemeLeptonXComponents.HttpError;
  protected readonly status = signal(0);

  readonly lastNavigation = this.routerEvents.previousNavigation;
  readonly currentNavigation = this.routerEvents.currentNavigation;

  protected readonly statusText = computed(
    () => HTTP_ERROR_STATUS[this.status()] ?? '',
  );

  protected readonly detail = computed(
    () => HTTP_ERROR_DETAIL[this.status()] ?? '',
  );

  constructor() {
    const { status } = this.activatedRoute.snapshot.queryParams;

    if (!isNaN(status)) {
      this.status.set(+status);
    }
  }

  goBack(): void {
    const url =
      this.status() === 404 ? this.currentNavigation() : this.lastNavigation();

    if (url) {
      this.router.navigateByUrl(url, { onSameUrlNavigation: 'reload' });
    } else {
      this.location.back();
    }

    this.destroy$?.next();
  }

  goHome(): void {
    this.router.navigateByUrl('/', { onSameUrlNavigation: 'reload' });
    this.destroy$?.next();
  }
}
