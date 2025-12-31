import { inject, Injectable } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import {
  SaasTenantConnectionStringsDto,
  SaasTenantDatabaseConnectionStringsDto,
} from '@volo/abp.ng.saas/proxy';

@Injectable()
export class ConnectionStringsFormBuilderService {
  protected readonly fb = inject(FormBuilder);

  protected connectionStringsForm(initial: SaasTenantConnectionStringsDto | null) {
    const group = this.fb.group({
      default: [initial?.default],
      databases: this.fb.array(
        (initial?.databases?.filter(f => !!f.connectionString) || []).map(database =>
          this.generateDbGroup(database),
        ),
      ),
    });

    return group;
  }

  generateForm(initial: SaasTenantConnectionStringsDto | null = null) {
    const group = this.fb.group({
      connectionStrings: this.connectionStringsForm(initial),
    });

    return group;
  }

  generateDbGroup(initial: SaasTenantDatabaseConnectionStringsDto) {
    if (!initial.connectionString) {
      return;
    }

    const group = this.fb.group({
      databaseName: [initial.databaseName],
      connectionString: [initial.connectionString],
      extraProperties: [initial.extraProperties],
    });

    return group;
  }
}
