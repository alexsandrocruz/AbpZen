/**
 * Angular Template Generators
 * Based on ABP Angular project structure
 */



// ============ UTILITY FUNCTIONS ============

export const toCamelCase = (str: string): string =>
  str.charAt(0).toLowerCase() + str.slice(1);

export const toKebabCase = (str: string): string =>
  str.replace(/([a-z])([A-Z])/g, '$1-$2').toLowerCase();

export const toPascalCase = (str: string): string =>
  str.charAt(0).toUpperCase() + str.slice(1);

// Map C# types to TypeScript types
export const toTsType = (type: string, nullable?: boolean): string => {
  const typeMap: Record<string, string> = {
    'string': 'string',
    'int': 'number',
    'long': 'number',
    'double': 'number',
    'decimal': 'number',
    'float': 'number',
    'bool': 'boolean',
    'guid': 'string',
    'datetime': 'string',
    'byte': 'number',
    'short': 'number',
    'char': 'string',
    'enum': 'number',
  };
  const tsType = typeMap[type.toLowerCase()] || 'unknown';
  return nullable ? `${tsType} | null` : tsType;
};

// ============ MODULE TEMPLATE ============

export function getAngularModuleTemplate(): string {
  return `import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { PageModule } from '@abp/ng.components/page';
import { {{ entity.name }}RoutingModule } from './{{ entity.name | kebabCase }}-routing.module';
import { {{ entity.name }}Component } from './{{ entity.name | kebabCase }}.component';

@NgModule({
  declarations: [{{ entity.name }}Component],
  imports: [
    SharedModule,
    {{ entity.name }}RoutingModule,
    PageModule,
  ],
})
export class {{ entity.name }}Module {}
`;
}

// ============ ROUTING MODULE TEMPLATE ============

export function getAngularRoutingModuleTemplate(): string {
  return `import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { {{ entity.name }}Component } from './{{ entity.name | kebabCase }}.component';
import { authGuard, permissionGuard } from '@abp/ng.core';

const routes: Routes = [
  { 
    path: '', 
    component: {{ entity.name }}Component,
    canActivate: [authGuard, permissionGuard],
    data: {
      requiredPolicy: '{{ project.name }}.{{ entity.name }}',
    },
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class {{ entity.name }}RoutingModule {}
`;
}

// ============ COMPONENT TS TEMPLATE ============

export function getAngularComponentTsTemplate(): string {
  return `import { ListService, PagedResultDto } from '@abp/ng.core';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Confirmation, ConfirmationService } from '@abp/ng.theme.shared';
import { {{ entity.name }}Service } from '@proxy/{{ entity.name | kebabCase }}';
import { {{ entity.name }}Dto } from '@proxy/{{ entity.name | kebabCase }}/dtos';

@Component({
  selector: 'app-{{ entity.name | kebabCase }}',
  templateUrl: './{{ entity.name | kebabCase }}.component.html',
  providers: [ListService],
})
export class {{ entity.name }}Component implements OnInit {
  {{ entity.name | camelCase }} = { items: [], totalCount: 0 } as PagedResultDto<{{ entity.name }}Dto>;

  isModalOpen = false;
  form: FormGroup;
  selected{{ entity.name }} = {} as {{ entity.name }}Dto;

  constructor(
    public readonly list: ListService,
    private {{ entity.name | camelCase }}Service: {{ entity.name }}Service,
    private fb: FormBuilder,
    private confirmation: ConfirmationService
  ) {}

  ngOnInit() {
    const streamCreator = (query) => this.{{ entity.name | camelCase }}Service.getList(query);

    this.list.hookToQuery(streamCreator).subscribe((response) => {
      this.{{ entity.name | camelCase }} = response;
    });
  }

  create{{ entity.name }}() {
    this.selected{{ entity.name }} = {} as {{ entity.name }}Dto;
    this.buildForm();
    this.isModalOpen = true;
  }

  edit{{ entity.name }}(id: string) {
    this.{{ entity.name | camelCase }}Service.get(id).subscribe(({{ entity.name | camelCase }}) => {
      this.selected{{ entity.name }} = {{ entity.name | camelCase }};
      this.buildForm();
      this.isModalOpen = true;
    });
  }

  delete{{ entity.name }}(id: string) {
    this.confirmation.warn('::AreYouSureToDelete', '::AreYouSure').subscribe((status) => {
      if (status === Confirmation.Status.confirm) {
        this.{{ entity.name | camelCase }}Service.delete(id).subscribe(() => this.list.get());
      }
    });
  }

  buildForm() {
    this.form = this.fb.group({
      {% for field in entity.fields %}{% if field.showInForm != false %}
      {{ field.name | camelCase }}: [
        this.selected{{ entity.name }}.{{ field.name | camelCase }} || {% if field.type == 'bool' %}false{% elsif field.type == 'string' %}''{% else %}null{% endif %}, 
        [{% if field.isRequired %}Validators.required{% endif %}{% if field.maxLength %}, Validators.maxLength({{ field.maxLength }}){% endif %}]
      ],
      {% endif %}{% endfor %}
    });
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const request = this.selected{{ entity.name }}.id
      ? this.{{ entity.name | camelCase }}Service.update(this.selected{{ entity.name }}.id, this.form.value)
      : this.{{ entity.name | camelCase }}Service.create(this.form.value);

    request.subscribe(() => {
      this.isModalOpen = false;
      this.form.reset();
      this.list.get();
    });
  }
}
`;
}

// ============ COMPONENT HTML TEMPLATE ============

export function getAngularComponentHtmlTemplate(): string {
  return `<abp-page [title]="'::{{ entity.pluralName }}' | abpLocalization">
  <abp-page-toolbar>
    <button
      *abpPermission="'{{ project.name }}.{{ entity.name }}.Create'"
      class="btn btn-primary"
      type="button"
      (click)="create{{ entity.name }}()"
    >
      <i class="fa fa-plus me-1"></i>
      <span>{{ "{{" }} '::New{{ entity.name }}' | abpLocalization {{ "}}" }}</span>
    </button>
  </abp-page-toolbar>

  <div class="card">
    <div class="card-body">
      <ngx-datatable [rows]="{{ entity.name | camelCase }}.items" [count]="{{ entity.name | camelCase }}.totalCount" [list]="list" default>
        <ngx-datatable-column
          [name]="'::Actions' | abpLocalization"
          [maxWidth]="150"
          [sortable]="false"
        >
          <ng-template let-row="row" ngx-datatable-cell-template>
            <div ngbDropdown container="body" class="d-inline-block">
              <button
                class="btn btn-primary btn-sm dropdown-toggle"
                data-toggle="dropdown"
                aria-haspopup="true"
                ngbDropdownToggle
              >
                <i class="fa fa-cog me-1"></i>{{ '::Actions' | abpLocalization }}
              </button>
              <div ngbDropdownMenu>
                <button
                  *abpPermission="'{{ project.name }}.{{ entity.name }}.Edit'"
                  ngbDropdownItem
                  (click)="edit{{ entity.name }}(row.id)"
                >
                  {{ '::Edit' | abpLocalization }}
                </button>
                <button
                  *abpPermission="'{{ project.name }}.{{ entity.name }}.Delete'"
                  ngbDropdownItem
                  (click)="delete{{ entity.name }}(row.id)"
                >
                  {{ '::Delete' | abpLocalization }}
                </button>
              </div>
            </div>
          </ng-template>
        </ngx-datatable-column>

        {% for field in entity.fields %}{% if field.showInGrid != false %}
        <ngx-datatable-column [name]="'::{{ field.label | default: field.name }}' | abpLocalization" prop="{{ field.name | camelCase }}">
          {% if field.type == 'datetime' %}
          <ng-template let-row="row" ngx-datatable-cell-template>
            {{ "{{" }} row.{{ field.name | camelCase }} | date {{ "}}" }}
          </ng-template>
          {% endif %}
          {% if field.type == 'bool' %}
          <ng-template let-row="row" ngx-datatable-cell-template>
            <i class="fa" [ngClass]="row.{{ field.name | camelCase }} ? 'fa-check text-success' : 'fa-times text-danger'"></i>
          </ng-template>
          {% endif %}
        </ngx-datatable-column>
        {% endif %}{% endfor %}
      </ngx-datatable>
    </div>
  </div>
</abp-page>

<abp-modal [(visible)]="isModalOpen">
  <ng-template #abpHeader>
    <h3>{{ "{{" }} (selected{{ entity.name }}.id ? '::Edit' : '::New{{ entity.name }}') | abpLocalization {{ "}}" }}</h3>
  </ng-template>

  <ng-template #abpBody>
    <form [formGroup]="form" (ngSubmit)="save()">
      {% for field in entity.fields %}{% if field.showInForm != false %}
      <div class="form-group">
        <label for="{{ field.name | kebabCase }}">{{ "{{" }} '::{{ field.label | default: field.name }}' | abpLocalization {{ "}}" }}</label><span>{% if field.isRequired %} * {% endif %}</span>
        {% if field.type == 'bool' %}
        <div class="custom-control custom-checkbox">
          <input type="checkbox" class="custom-control-input" id="{{ field.name | kebabCase }}" formControlName="{{ field.name | camelCase }}" />
          <label class="custom-control-label" for="{{ field.name | kebabCase }}">{{ "{{" }} '::{{ field.label | default: field.name }}' | abpLocalization {{ "}}" }}</label>
        </div>
        {% elsif field.isTextArea %}
        <textarea id="{{ field.name | kebabCase }}" class="form-control" formControlName="{{ field.name | camelCase }}"></textarea>
        {% elsif field.type == 'datetime' %}
        <input type="date" id="{{ field.name | kebabCase }}" class="form-control" formControlName="{{ field.name | camelCase }}" />
        {% elsif field.type == 'int' or field.type == 'long' or field.type == 'double' or field.type == 'decimal' %}
        <input type="number" id="{{ field.name | kebabCase }}" class="form-control" formControlName="{{ field.name | camelCase }}" />
        {% else %}
        <input type="text" id="{{ field.name | kebabCase }}" class="form-control" formControlName="{{ field.name | camelCase }}" autofocus />
        {% endif %}
      </div>
      {% endif %}{% endfor %}
    </form>
  </ng-template>

  <ng-template #abpFooter>
    <button type="button" class="btn btn-secondary" abpClose>
      {{ '::Cancel' | abpLocalization }}
    </button>
    <button class="btn btn-primary" (click)="save()" [disabled]="form.invalid">
      <i class="fa fa-check mr-1"></i>
      {{ '::Save' | abpLocalization }}
    </button>
  </ng-template>
</abp-modal>
`;
}

// ============ ROUTE PROVIDER TEMPLATE ============

export function getAngularRouteProviderTemplate(): string {
  return `import { eLayoutType, RoutesService } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routes: RoutesService) {
  return () => {
    routes.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/dashboard',
        name: '::Menu:Dashboard',
        iconClass: 'fas fa-chart-line',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: '{{ project.name }}.Dashboard.Host || LeptonX.Dashboard.Tenant',
      },
      {% for ent in entities %}
      {
        path: '/{{ ent.name | kebabCase }}s',
        name: '::Menu:{{ ent.pluralName }}',
        iconClass: 'fas fa-list',
        order: {{ forloop.index | plus: 2 }},
        layout: eLayoutType.application,
        requiredPolicy: '{{ project.name }}.{{ ent.name }}',
      },
      {% endfor %}
    ]);
  };
}
`;
}

