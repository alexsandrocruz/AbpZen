import { BehaviorSubject, Subject } from 'rxjs';

export class DataStore<State> {
  private state$ = new BehaviorSubject<State>(this.initialState);

  private update$ = new Subject<Partial<State>>();

  get state() {
    return this.state$.value;
  }

  constructor(private initialState: State) {}
  patch(state: Partial<State>) {
    let patchedState = state as State;

    if (typeof state === 'object' && !Array.isArray(state)) {
      patchedState = { ...this.state, ...state };
    }

    this.state$.next(patchedState);
    this.update$.next(patchedState);
  }
  get() {
    return this.state$;
  }
}
