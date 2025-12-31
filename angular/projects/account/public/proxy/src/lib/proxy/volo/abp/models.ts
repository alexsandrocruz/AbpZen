
export interface NameValue<T> {
  name?: string;
  value: T;
}

export interface NameValue<T = "string"> {
  name?: string;
  value: T;
}
