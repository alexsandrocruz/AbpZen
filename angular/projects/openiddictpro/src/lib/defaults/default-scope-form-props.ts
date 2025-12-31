import { Validators } from '@angular/forms';
import { ePropType, FormProp } from '@abp/ng.components/extensible';
import { Scopes } from '@volo/abp.ng.openiddictpro/proxy';

export const DEFAULT_SCOPE_CREATE_FORM_PROPS = FormProp.createMany<Scopes.Dtos.ScopeDto>([
  {
    type: ePropType.String,
    name: 'name',
    displayName: 'AbpOpenIddict::Name',
    id: 'name',
    validators: () => [Validators.required, Validators.pattern(/^\S+$/)],
  },

  {
    type: ePropType.String,
    name: 'displayName',
    displayName: 'AbpOpenIddict::DisplayName',
    id: 'displayName',
  },
  {
    type: ePropType.String,
    name: 'description',
    displayName: 'AbpOpenIddict::Description',
    id: 'description',
  },
  {
    type: ePropType.Text,
    name: 'resources',
    displayName: 'AbpOpenIddict::Resources',
    id: 'resources',
  },
]);

export const DEFAULT_SCOPES_FORM_PROPS = DEFAULT_SCOPE_CREATE_FORM_PROPS;
