import { Observable } from 'rxjs';
import { Setting } from './setting.model';

export interface SettingsService {
  settings$: Observable<Setting[]>;
  selectedSettings$: Observable<Setting[]>;
}
