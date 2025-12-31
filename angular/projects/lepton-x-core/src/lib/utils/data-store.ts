import { BehaviorSubject, Subject } from 'rxjs';
import { map, distinctUntilChanged, filter } from 'rxjs/operators';

export class DataStore<State> {
  private state$ = new BehaviorSubject<State>(this.initialState);

  private update$ = new Subject<Partial<State>>();

  get state() {
    return this.state$.value;
  }

  sliceState = <Slice>(
    selector: (state: State) => Slice,
    compareFn = (s1: Slice, s2: Slice) => s1 === s2
  ) => this.state$.pipe(map(selector), distinctUntilChanged(compareFn));

  sliceUpdate = <Slice>(
    selector: (state: Partial<State>) => Slice,
    filterFn = (x: Slice) => x !== undefined
  ) => this.update$.pipe(map(selector), filter(filterFn));

  constructor(private initialState: State) {}

  patch(state: Partial<State>) {
    let patchedState = state as State;

    if (typeof state === 'object' && !Array.isArray(state)) {
      patchedState = { ...this.state, ...state };
    }

    this.state$.next(patchedState);
    this.update$.next(patchedState);
  }

  set(state: State) {
    this.state$.next(state);
    this.update$.next(state);
  }

  reset() {
    this.set(this.initialState);
  }
}
