import { ChangeDetectionStrategy, Component, inject, input, model } from '@angular/core';
import {
  AbstractControl,
  ControlContainer,
  FormArray,
  FormGroup,
  FormsModule,
} from '@angular/forms';
import { combineLatest, tap } from 'rxjs';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import { LocalizationModule } from '@abp/ng.core';
import { ToasterService } from '@abp/ng.theme.shared';
import {
  SaasTenantDatabaseConnectionStringsDto,
  SaasTenantDto,
  TenantService,
} from '@volo/abp.ng.saas/proxy';
import { parentFormGroupProvider } from '../../../providers';
import { ConnectionStringsFormBuilderService } from '../../../services';

@Component({
  standalone: true,
  selector: 'abp-module-specific-db',
  templateUrl: './module-specific-db.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule, NgbPopoverModule, LocalizationModule],
  viewProviders: [parentFormGroupProvider],
})
export class ModuleSpecificDbComponent {
  protected readonly controlContainer = inject(ControlContainer);
  protected readonly service = inject(TenantService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly conStrFormBuilderService = inject(ConnectionStringsFormBuilderService);

  selectedModule: string | null = null;
  databaseConnectionString: string;

  databases = model<SaasTenantDatabaseConnectionStringsDto[]>([]);
  moduleSpecificDatabase = model(false);
  useSharedDatabase = model(true);
  selected = input<SaasTenantDto>();

  get modules() {
    const dbs = (this.group.get('connectionStrings').get('databases') as FormArray)
      .value as SaasTenantDatabaseConnectionStringsDto[];
    const dbNames = dbs.map(db => db.databaseName);
    return this.databases()?.filter(f => dbNames?.findIndex(db => db === f.databaseName) < 0) || [];
  }

  protected get group(): FormGroup {
    return <FormGroup>this.controlContainer.control;
  }

  protected get defaultControl(): AbstractControl {
    return this.group.get('connectionStrings').get('default');
  }

  protected get databasesControl(): FormArray {
    return this.group.get('connectionStrings').get('databases') as FormArray;
  }

  protected init() {
    if (this.selected()?.id) {
      combineLatest([this.getConnectionStrings(), this.getDatabases()]).subscribe();
    } else {
      this.getDatabases().subscribe();
    }
  }

  protected getDatabases() {
    return this.service.getDatabases().pipe(
      tap(response => {
        const { databases } = response || {};
        const arr = (databases || []).map(db => ({
          id: null,
          databaseName: db,
          connectionString: null,
          extraProperties: {},
        }));

        this.databases.set(arr);
        this.selectFirstDatabase();
      }),
    );
  }

  protected getConnectionStrings() {
    return this.service.getConnectionStrings(this.selected().id).pipe(
      tap(response => {
        this.useSharedDatabase.set(
          !response.default && !response.databases.some(db => !!db.connectionString),
        );

        this.defaultControl?.setValue(response.default);

        this.databases.set(response.databases);

        if (this.databases().find(db => !!db.connectionString)) {
          this.moduleSpecificDatabase.set(true);
        }

        if (response.databases.find(db => !!db.connectionString)) {
          for (const db of response.databases.filter(f => f.connectionString)) {
            this.databasesControl.push(this.conStrFormBuilderService.generateDbGroup(db));
          }
        }

        this.selectFirstDatabase();
      }),
    );
  }

  protected selectFirstDatabase() {
    this.selectedModule = this.modules[0]?.databaseName || null;
  }

  ngOnInit() {
    this.init();
  }

  add() {
    if (!this.databaseConnectionString || !this.selectedModule) {
      return;
    }

    const item = {
      ...this.databases().find(f => f.databaseName === this.selectedModule),
    };

    if (!item?.databaseName) {
      return;
    }

    item.connectionString = this.databaseConnectionString;

    const group = this.conStrFormBuilderService.generateDbGroup(item);
    this.databasesControl.push(group);
    this.databaseConnectionString = null;
    this.selectFirstDatabase();
  }

  remove(dbName: string) {
    const index = this.databasesControl.controls.findIndex(f => f.value.databaseName === dbName);
    if (index < 0) {
      return;
    }

    this.databasesControl.removeAt(index);
    this.selectFirstDatabase();
  }

  checkConnectionString(dbConnection?: string) {
    if (!dbConnection && !this.defaultControl.value) {
      return this.toasterService.error('Saas::InvalidConnectionString');
    }

    this.service
      .checkConnectionString(dbConnection || this.defaultControl.value)
      .subscribe(response => {
        if (response) {
          return this.toasterService.success('Saas::ValidConnectionString');
        }

        this.toasterService.error('Saas::InvalidConnectionString');
      });
  }
}
