import { of } from 'rxjs';

export const getSubscriptionStatus = (condition: boolean, defaultValue: any) => {
  return of({ isSubscription: condition, defaultValue });
};
