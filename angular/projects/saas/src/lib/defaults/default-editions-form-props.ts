import { ePropType, FormProp } from '@abp/ng.components/extensible';
import { Validators } from '@angular/forms';
import { EditionDto } from '@volo/abp.ng.saas/proxy';
import { map } from 'rxjs/operators';
import { EditionsComponent } from '../components/editions/editions.component';

export const DEFAULT_EDITIONS_FORM_PROPS = FormProp.createMany<EditionDto>([
  {
    type: ePropType.String,
    name: 'displayName',
    displayName: 'Saas::EditionName',
    validators: () => [Validators.required, Validators.maxLength(256)],
  },
  {
    type: ePropType.Enum,
    name: 'planId',
    displayName: 'Saas::PlanId',
    visible: data => {
      const editionComponent = data.getInjected(EditionsComponent);
      return !!editionComponent.plans$.value.length;
    },
    options: data => {
      const editionComponent = data.getInjected(EditionsComponent);
      return editionComponent.plans$.pipe(
        map(options =>
          [{ key: '-', value: null }].concat(
            options.map(plan => ({
              key: plan.name,
              value: plan.id,
            })),
          ),
        ),
      );
    },
  },
]);
