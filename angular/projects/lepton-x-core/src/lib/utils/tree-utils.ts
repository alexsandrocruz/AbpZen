import { Observable } from 'rxjs';
import { isArray } from './common';

export function createGroupMap<T extends { group?: string }>(
  list: T[],
  othersGroupKey: string,
  skipGroupCheck: boolean = false
): Map<string, T[]> | undefined {
  if (
    !skipGroupCheck &&
    (!isArray(list) || !list.some((node) => Boolean(node.group)))
  )
    return undefined;

  const mapGroup = new Map<string, T[]>();

  for (const node of list) {
    const group = node?.group || othersGroupKey;
    if (typeof group !== 'string') {
      throw new Error(`Invalid group: ${group}`);
    }

    const items = mapGroup.get(group) || [];
    items.push(node);
    mapGroup.set(group, items);
  }
  return mapGroup;
}

export function getItemsFromGroup<S extends { items: R[] }, R>(
  list: S[],
  pred?: ItemPropPredicate<R>
): R[] | undefined {
  return list?.reduce<R[]>(
    (acc, { items }) => [...acc, ...(pred ? items.filter(pred) : items)],
    []
  );
}

export type ItemPropPredicate<T> = (
  prop: T
) => boolean | Promise<boolean> | Observable<boolean>;
