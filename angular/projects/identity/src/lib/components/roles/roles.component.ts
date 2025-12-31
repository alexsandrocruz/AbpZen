import {
  Component,
  DestroyRef,
  inject,
  Injector,
  OnInit,
  TemplateRef,
  ViewChild,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ListService, PagedResultDto } from '@abp/ng.core';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import { EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import {
  GetIdentityRoleListInput,
  IdentityRoleDto,
  IdentityRoleService,
} from '@volo/abp.ng.identity/proxy';
import { eIdentityComponents } from '../../enums/components';
import { RoleVisibleChange } from './role-edit.modal';

@Component({
  selector: 'abp-roles',
  templateUrl: './roles.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eIdentityComponents.Roles,
    },
  ],
})
export class RolesComponent implements OnInit {
  protected readonly list = inject(ListService<GetIdentityRoleListInput>);
  protected readonly service = inject(IdentityRoleService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly destroyRef = inject(DestroyRef);
  protected readonly injector = inject(Injector);
  protected readonly toasterService = inject(ToasterService);

  data: PagedResultDto<IdentityRoleDto> = { items: [], totalCount: 0 };

  selected: IdentityRoleDto;

  isModalVisible: boolean;
  visibleRoleDelete = false;
  visibleMove = false;
  visiblePermissions = false;

  providerKey: string;

  modalBusy = false;

  visibleClaims = false;

  claimSubject = {} as { id: string; title: string; type: 'roles' | 'users' };

  @ViewChild('modalContent')
  modalContent: TemplateRef<any>;

  onVisiblePermissionChange = (value: boolean) => {
    this.visiblePermissions = value;
  };

  private hookToQuery() {
    this.list.hookToQuery(query => this.service.getList(query)).subscribe(res => (this.data = res));
  }

  ngOnInit() {
    this.hookToQuery();
  }

  onAdd() {
    this.isModalVisible = true;
  }

  onEdit(id: string) {
    this.service.get(id).subscribe(selectedRole => {
      this.selected = selectedRole;
      this.isModalVisible = true;
    });
  }

  delete(role: IdentityRoleDto) {
    this.selected = role;
    this.visibleRoleDelete = true;
  }

  onVisibleDeleteChange(v: RoleVisibleChange) {
    if (v.visible) {
      return;
    }
    if (v.refresh) {
      this.list.get();
    }
    this.selected = null;
    this.visibleRoleDelete = false;
  }

  async moveAllUsers(role: IdentityRoleDto) {
    if (role.userCount === 0) {
      this.confirmationService
        .warn('AbpIdentity::ThereIsNoUsersCurrentlyInThisRole', 'AbpUi::Warning', {
          hideCancelBtn: true,
          yesText: 'AbpUi::Ok',
        })
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe();
      return;
    }
    this.selected = role;
    this.visibleMove = true;
  }

  onVisibleMoveChange(v: RoleVisibleChange) {
    if (v.visible) {
      return;
    }
    if (v.refresh) {
      this.list.get();
    }
    this.selected = null;
    this.visibleMove = false;
  }

  onVisibleModalChange(visibilityChange: RoleVisibleChange) {
    if (visibilityChange.visible) {
      return;
    }

    if (visibilityChange.refresh) {
      this.list.get();
    }

    this.selected = null;
    this.isModalVisible = false;
  }

  onManageClaims(role: IdentityRoleDto) {
    this.selected = role;
    this.claimSubject = {
      id: role.id,
      title: role.name,
      type: 'roles',
    };

    this.visibleClaims = true;
  }

  openPermissionsModal(providerKey: string) {
    this.providerKey = providerKey;
    //TODO: remove timeout if not need, try to use signal
    setTimeout(() => {
      this.visiblePermissions = true;
    }, 0);
  }
}
