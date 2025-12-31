import {
  ExtensionsService,
  getObjectExtensionEntitiesFromStore,
  mapEntitiesToContributors,
  mergeWithDefaultActions,
  mergeWithDefaultProps,
} from '@abp/ng.components/extensible';
import { Injector, inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { map, tap } from 'rxjs';
import { eAuditLoggingComponents } from '../enums';
import {
  AuditLoggingEntityActionContributors,
  AuditLoggingToolbarActionContributors,
  AuditLoggingEntityPropContributors,
} from '../models';
import {
  AUDIT_LOGGING_ENTITY_ACTION_CONTRIBUTORS,
  AUDIT_LOGGING_TOOLBAR_ACTION_CONTRIBUTORS,
  AUDIT_LOGGING_ENTITY_PROP_CONTRIBUTORS,
  DEFAULT_AUDIT_LOGGING_ENTITY_ACTIONS,
  DEFAULT_AUDIT_LOGGING_TOOLBAR_ACTIONS,
  DEFAULT_AUDIT_LOGGING_ENTITY_PROPS,
} from '../tokens';

export const auditLoggingExtensionsResolver: ResolveFn<any> = () => {
  const injector = inject(Injector);

  const extensions: ExtensionsService = injector.get(ExtensionsService);
  const actionContributors: AuditLoggingEntityActionContributors =
    injector.get(AUDIT_LOGGING_ENTITY_ACTION_CONTRIBUTORS, null) || {};
  const toolbarContributors: AuditLoggingToolbarActionContributors =
    injector.get(AUDIT_LOGGING_TOOLBAR_ACTION_CONTRIBUTORS, null) || {};
  const propContributors: AuditLoggingEntityPropContributors =
    injector.get(AUDIT_LOGGING_ENTITY_PROP_CONTRIBUTORS, null) || {};

  return getObjectExtensionEntitiesFromStore(injector, 'AuditLogging').pipe(
    map(entities => ({
      [eAuditLoggingComponents.AuditLogs]: entities.AuditLog,
    })),
    mapEntitiesToContributors(injector, 'AuditLogging'),
    tap(objectExtensionContributors => {
      mergeWithDefaultActions(
        extensions.entityActions,
        DEFAULT_AUDIT_LOGGING_ENTITY_ACTIONS,
        actionContributors,
      );
      mergeWithDefaultActions(
        extensions.toolbarActions,
        DEFAULT_AUDIT_LOGGING_TOOLBAR_ACTIONS,
        toolbarContributors,
      );
      mergeWithDefaultProps(
        extensions.entityProps,
        DEFAULT_AUDIT_LOGGING_ENTITY_PROPS,
        objectExtensionContributors.prop,
        propContributors,
      );
    }),
  );
};
