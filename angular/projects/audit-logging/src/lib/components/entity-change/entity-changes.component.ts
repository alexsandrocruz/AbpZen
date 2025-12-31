import { ListService, mapEnumToOptions, PagedResultDto, TrackByService } from '@abp/ng.core';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { DateAdapter } from '@abp/ng.theme.shared';
import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
} from '@angular/core';
import { UntypedFormBuilder } from '@angular/forms';
import { NgbDateAdapter } from '@ng-bootstrap/ng-bootstrap';
import {
  AuditLogsService,
  EntityChangeDto,
  EntityChangeType,
} from '@volo/abp.ng.audit-logging/proxy';
import { eAuditLoggingComponents } from '../../enums/components';

@Component({
  selector: 'abp-entity-changes',
  templateUrl: './entity-changes.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eAuditLoggingComponents.EntityChanges,
    },
    { provide: NgbDateAdapter, useClass: DateAdapter },
  ],
})
export class EntityChangesComponent implements AfterViewInit {
  form = this.fb.group({
    entityChangeType: [''],
    entityTypeFullName: [''],
    times: [{}],
  });

  private response = { items: [], totalCount: 0 } as PagedResultDto<EntityChangeDto>;

  get data() {
    return this.response.items;
  }

  get count() {
    return this.response.totalCount;
  }

  changeType = EntityChangeType;

  changeTypes = mapEnumToOptions(EntityChangeType);

  constructor(
    public readonly list: ListService,
    private fb: UntypedFormBuilder,
    private auditLogsService: AuditLogsService,
    public readonly track: TrackByService,
    private cdr: ChangeDetectorRef,
  ) {}

  ngAfterViewInit() {
    this.hookToQuery();
    this.list.get();
  }

  private hookToQuery() {
    this.list
      .hookToQuery(query => {
        const value = {
          ...this.form.value,
          ...this.form.value.times,
        };
        return this.auditLogsService.getEntityChanges({ ...value, ...query });
      })
      .subscribe(res => {
        this.response = res;
        this.cdr.detectChanges();
      });
  }

  handleSubmit() {
    this.list.get();
  }
}
