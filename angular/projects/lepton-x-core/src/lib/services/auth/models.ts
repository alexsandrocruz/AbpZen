import { Observable } from 'rxjs';

export interface AuthService {
  navigateToLogin(): void;
  isUserExists$: Observable<boolean>;
}
