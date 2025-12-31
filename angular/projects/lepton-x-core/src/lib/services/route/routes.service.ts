import { toSignal } from '@angular/core/rxjs-interop';
import { Injectable, inject } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class RoutesService {
  protected readonly router = inject(Router);

  readonly currentNavigation = toSignal(
    this.router.events.pipe(
      filter((e) => e instanceof NavigationEnd),
      //TODO: location object might be problem in the future for SSR
      map(() => location.pathname),
    ),
    { initialValue: location.pathname },
  );
}
