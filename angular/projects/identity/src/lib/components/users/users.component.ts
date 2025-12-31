import {
  Component,
  Injector,
  OnInit,
  TemplateRef,
  TrackByFunction,
  ViewChild,
  inject,
  signal,
  computed,
} from '@angular/core';
import {
  AbstractControl,
  UntypedFormArray,
  UntypedFormBuilder,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { finalize, map, switchMap, take, tap } from 'rxjs/operators';
import { BaseNode, TreeAdapter } from '@abp/ng.components/tree';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { ConfigStateService, ListService, PagedResultDto, SubscriptionService } from '@abp/ng.core';
import { Confirmation, ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import {
  GetIdentityUsersInput,
  IdentityRoleDto,
  IdentityRoleService,
  IdentityUserDto,
  IdentityUserService,
  OrganizationUnitDto,
  OrganizationUnitService,
  OrganizationUnitWithDetailsDto,
  ImportUsersFromFileType,
  OrganizationUnitLookupDto,
  IdentityRoleLookupDto,
} from '@volo/abp.ng.identity/proxy';
import { eIdentityComponents } from '../../enums/components';
import { identityTwoFactorBehaviourOptions } from '../../enums/two-factor-behaviour';
import { ExcelService } from '../../services/excel.service';

@Component({
  selector: 'abp-users',
  templateUrl: './users.component.html',
  providers: [
    ListService,
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eIdentityComponents.Users,
    },
    ExcelService,
  ],
})
export class UsersComponent implements OnInit {
  protected readonly subscription = inject(SubscriptionService);
  protected readonly confirmationService = inject(ConfirmationService);
  protected readonly fb = inject(UntypedFormBuilder);
  protected readonly toasterService = inject(ToasterService);
  protected readonly injector = inject(Injector);
  protected readonly configState = inject(ConfigStateService);
  protected readonly roleService = inject(IdentityRoleService);
  protected readonly organizationUnitService = inject(OrganizationUnitService);
  protected readonly excelService = inject(ExcelService);
  public readonly list = inject(ListService<GetIdentityUsersInput>);
  public readonly service = inject(IdentityUserService);

  readonly #roleCount = signal<number>(0);
  roleCount = computed(() => this.#roleCount());

  protected userRoles: IdentityRoleDto[];

  data: PagedResultDto<IdentityUserDto> = { items: [], totalCount: 0 };

  @ViewChild('modalContent')
  modalContent: TemplateRef<any>;

  form: UntypedFormGroup;

  selected: IdentityUserDto;

  roles: IdentityRoleDto[];

  selectedOrganizationUnits: OrganizationUnitDto[];

  visiblePermissions = false;

  providerKey: string;

  isModalVisible: boolean;

  isSetPasswordModalVisible: boolean;

  modalBusy = false;

  visibleViewDetails = false;

  visibleUploadFile = false;

  visibleSessionsModal = signal(false);

  fileType: ImportUsersFromFileType;

  visibleClaims = false;

  fieldTextType: boolean;

  claimSubject = {} as { id: string; title: string; type: 'roles' | 'users' };

  filters = {} as GetIdentityUsersInput;

  organization = {
    response: {} as PagedResultDto<OrganizationUnitWithDetailsDto>,
    nodes: [],
    checkedKeys: [],
    expandedKeys: [],
    selectFn: () => false,
  };

  isLockModalVisible: boolean;

  twoFactor = {
    isModalVisible: false,
    checkboxValue: false,
    isOptional: false,
  };

  lockForm = this.fb.group({
    lockoutEnd: [new Date().toISOString(), [Validators.required]],
  });

  entityDisplayName: string | undefined;

  trackByFn: TrackByFunction<AbstractControl> = (index, item) => Object.keys(item)[0] || index;

  private patchRoleCount(): void {
    const count = this.rawRoleNames.filter(obj => Object.values(obj).includes(true)).length;
    this.#roleCount.set(count);
  }

  private get rawRoleNames() {
    return this.form.controls.roleNames?.getRawValue();
  }

  get roleGroups(): UntypedFormGroup[] {
    return ((this.form.get('roleNames') as UntypedFormArray)?.controls as UntypedFormGroup[]) || [];
  }

  ngOnInit() {
    const { key } = identityTwoFactorBehaviourOptions[0];
    this.twoFactor.isOptional =
      this.configState.getFeature('Identity.TwoFactor') === key &&
      this.configState.getSetting('Abp.Identity.TwoFactor.Behaviour') === key;

    this.hookToQuery();
  }

  isFromOrgUnit = (roleId: string) =>
    this.selectedOrganizationUnits.some(org => org.roles.some(f => f.roleId === roleId));

  onVisiblePermissionChange = (value: boolean) => {
    this.visiblePermissions = value;
  };

  clearFilters() {
    this.filters = {} as GetIdentityUsersInput;
  }

  protected hookToQuery() {
    this.list
      .hookToQuery(query => {
        return this.service.getList({
          ...query,
          ...this.filters,
        });
      })
      .subscribe(res => (this.data = res));
  }

  buildForm() {
    const data = new FormPropData(this.injector, this.selected);
    this.form = generateFormFromProps(data);

    this.service.getAssignableRoles().subscribe(({ items }) => {
      this.roles = items;
      this.form.addControl(
        'roleNames',
        this.fb.array(
          this.roles.map(role =>
            this.fb.group({
              [role.name]: [
                this.selected.id
                  ? !!this.userRoles?.find(userRole => userRole.id === role.id)
                  : role.isDefault,
              ],
            }),
          ),
        ),
      );

      this.patchRoleCount();
      this.subscription.addOne(this.form.controls['roleNames'].valueChanges, () =>
        this.patchRoleCount(),
      );
    });

    this.service.getAvailableOrganizationUnits().subscribe(res => {
      this.organization.response = res;
      this.organization.nodes = new TreeAdapter(res.items as BaseNode[]).getTree();
      this.organization.expandedKeys = res.items.map(item => item.id);
      this.organization.checkedKeys = this.selectedOrganizationUnits.map(unit => unit.id);
    });
  }

  openModal() {
    this.buildForm();
    this.isModalVisible = true;
  }

  onAdd() {
    this.selected = {} as IdentityUserDto;
    this.userRoles = [];
    this.selectedOrganizationUnits = [];
    this.openModal();
  }

  onEdit(id: string) {
    this.service
      .get(id)
      .pipe(
        tap(selectedUser => (this.selected = selectedUser)),
        switchMap(() => this.service.getRoles(id)),
        tap(res => (this.userRoles = res.items || [])),
        switchMap(() => this.service.getOrganizationUnits(id)),
        tap(res => (this.selectedOrganizationUnits = res)),
        take(1),
      )
      .subscribe(() => this.openModal());
  }

  save() {
    if (!this.form.valid) return;
    this.modalBusy = true;
    const { roleNames } = this.form.value;
    const mappedRoleNames =
      roleNames?.filter(role => !!role[Object.keys(role)[0]])?.map(role => Object.keys(role)[0]) ||
      [];

    const { id } = this.selected;

    (id
      ? this.service.update(id, {
          ...this.selected,
          ...this.form.value,
          roleNames: mappedRoleNames,
          organizationUnitIds: this.organization.checkedKeys,
        })
      : this.service.create({
          ...this.form.value,
          roleNames: mappedRoleNames,
          organizationUnitIds: this.organization.checkedKeys,
        })
    )
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.list.get();
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.isModalVisible = false;
      });
  }

  delete(id: string, userName: string) {
    this.confirmationService
      .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpUi::AreYouSure', {
        messageLocalizationParams: [userName],
      })
      .subscribe((status: Confirmation.Status) => {
        if (status === Confirmation.Status.confirm) {
          this.service.delete(id).subscribe(() => {
            this.toasterService.success('AbpUi::DeletedSuccessfully');
            this.list.get();
          });
        }
      });
  }

  onManageClaims(user: IdentityUserDto) {
    this.selected = user;
    this.claimSubject = {
      id: user.id,
      title: user.userName,
      type: 'users',
    };

    this.visibleClaims = true;
  }

  unlock(id: string) {
    this.service.unlock(id).subscribe(() => {
      this.toasterService.success('AbpIdentity::UserUnlocked');
      this.list.get();
    });
  }

  openPermissionsModal(providerKey: string, userName?: string) {
    this.providerKey = providerKey;
    this.entityDisplayName = userName;
    setTimeout(() => {
      this.visiblePermissions = true;
    }, 0);
  }

  setTwoFactor() {
    this.modalBusy = true;
    this.service
      .setTwoFactorEnabled(this.selected.id, this.twoFactor.checkboxValue)
      .pipe(finalize(() => (this.modalBusy = false)))
      .subscribe(() => {
        this.twoFactor.isModalVisible = false;
        this.toasterService.success('AbpUi::SavedSuccessfully');
        this.list.get();
      });
  }

  getOrganizationUnitLookup() {
    return () =>
      this.service
        .getOrganizationUnitLookup()
        .pipe(map(items => ({ items }) as PagedResultDto<OrganizationUnitLookupDto>));
  }

  getRolesLookup() {
    return () =>
      this.service
        .getRoleLookup()
        .pipe(map(items => ({ items }) as PagedResultDto<IdentityRoleLookupDto>));
  }

  viewDetails(user: IdentityUserDto): void {
    this.visibleViewDetails = true;
    this.selected = user;
  }

  openSessionsModal(user: IdentityUserDto) {
    this.visibleSessionsModal.set(true);
    this.selected = user;
  }

  exportExcel() {
    this.excelService.exportExcel();
  }

  exportCSV() {
    this.excelService.exportCSV();
  }

  openUploadFileModal(fileType: ImportUsersFromFileType) {
    this.fileType = fileType;
    this.visibleUploadFile = true;
  }

  resetList() {
    this.list.get();
  }

  lockModalVisibleChange($event: boolean) {
    if ($event) {
      return;
    }
    this.isLockModalVisible = $event;
    this.resetList();
  }

  setPasswordModalChange($event: boolean) {
    this.isSetPasswordModalVisible = $event;
  }
}
