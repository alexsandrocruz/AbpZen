import { LpxStyles } from '../style/models';

export function styleLoadFactory(
  styleList: LpxStyles,
  layoutStyles: LpxStyles,
) {
  styleList.push({
    bundleName: 'ng-bundle',
  });
  styleList.push({
    bundleName: 'font-bundle',
  });
  return [...styleList, ...layoutStyles];
}
