import { BaseNode, DropEvent, TreeAdapter } from '@abp/ng.components/tree';
import { ListResultDto, TreeNode } from '@abp/ng.core';
import { ConfirmationService, ToasterService } from '@abp/ng.theme.shared';
import {
  EXTENSIONS_IDENTIFIER,
  FormPropData,
  generateFormFromProps,
} from '@abp/ng.components/extensible';
import { Component, Injector, OnDestroy, OnInit, inject } from '@angular/core';
import { UntypedFormBuilder, UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import {
  OrganizationUnitService,
  OrganizationUnitUpdateDto,
  OrganizationUnitWithDetailsDto,
} from '@volo/abp.ng.identity/proxy';
import { finalize } from 'rxjs/operators';
import { eIdentityComponents } from '../../enums/components';
import {
  OrganizationUnitsStateService,
  DeleteOrganizationUnitService,
  MoveAllUsersOfUnitModalService,
} from '../../services';

@Component({
  selector: 'abp-organization-units',
  templateUrl: './organization-units.component.html',
  providers: [
    {
      provide: EXTENSIONS_IDENTIFIER,
      useValue: eIdentityComponents.OrganizationUnits,
    },
    DeleteOrganizationUnitService,
    MoveAllUsersOfUnitModalService,
  ],
  styles: [
    `
      .fs-15px {
        font-size: 15px;
      }
    `,
  ],
})
export class OrganizationUnitsComponent implements OnInit, OnDestroy {
  protected readonly injector = inject(Injector);
  protected readonly service = inject(OrganizationUnitService);
  protected readonly fb = inject(UntypedFormBuilder);
  protected readonly confirmation = inject(ConfirmationService);
  protected readonly toasterService = inject(ToasterService);
  protected readonly deleteOrganizationUnitService = inject(DeleteOrganizationUnitService);
  protected readonly moveAllUsersOfUnitModalService = inject(MoveAllUsersOfUnitModalService);

  public readonly organizationUnitsStateService = inject(OrganizationUnitsStateService);

  organizationUnits: OrganizationUnitWithDetailsDto[] = [];
  nodes: TreeNode<BaseNode>[] = [];
  treeAdapter: TreeAdapter;
  expandedKeys: string[] = [];
  isNodeModalVisible: boolean;
  isModalBusy: boolean;
  organizationMembersKey = eIdentityComponents.OrganizationMembers;
  organizationRolesKey = eIdentityComponents.OrganizationRoles;
  loading: boolean;
  nodeForm: UntypedFormGroup;

  get selectedUnit() {
    return this.organizationUnitsStateService.getSelectedNode();
  }

  get nodeId() {
    return this.nodeForm.get('id')?.value;
  }

  ngOnInit() {
    this.get();
  }

  ngOnDestroy(): void {
    this.organizationUnitsStateService.removeSelectedNode();
  }

  refreshSelectedUnit() {
    if (this.selectedUnit && this.organizationUnits.length > 0) {
      const newUnit = this.organizationUnits.find(unit => unit.id === this.selectedUnit.id);
      this.setSelectedUnit(newUnit);
    }
  }

  get = () => {
    this.loading = true;
    this.service
      .getListAll()
      .pipe(finalize(() => (this.loading = false)))
      .subscribe((res: ListResultDto<OrganizationUnitWithDetailsDto>) => {
        this.organizationUnits = res.items;
        this.refreshSelectedUnit();

        this.treeAdapter = new TreeAdapter(
          res.items as OrganizationUnitWithDetailsDto[] as BaseNode[],
        );
        this.nodes = this.treeAdapter.getTree();
        this.expandedKeys = [...this.expandedKeys];
      });
  };

  buildForm(selected = {} as OrganizationUnitWithDetailsDto) {
    const data = new FormPropData(this.injector, selected);
    this.nodeForm = generateFormFromProps(data);
    this.nodeForm.addControl('parentId', new UntypedFormControl(undefined));
    this.nodeForm.addControl('id', new UntypedFormControl(undefined));
  }

  add() {
    this.buildForm();
    this.isNodeModalVisible = true;
  }

  edit(selected: OrganizationUnitWithDetailsDto) {
    this.buildForm(selected);
    this.nodeForm.patchValue({
      parentId: '',
      displayName: selected.displayName,
      id: selected.id,
    });
    this.isNodeModalVisible = true;
  }

  addSubUnit({ id }: OrganizationUnitWithDetailsDto) {
    this.buildForm();
    this.nodeForm.patchValue({ parentId: id, displayName: '', id: undefined });
    this.isNodeModalVisible = true;
    this.expandedKeys = this.expandedKeys.concat(id);
  }

  save() {
    if (this.nodeForm.invalid) return;

    const { id, ...form } = this.nodeForm.value;
    const request = id
      ? this.service.update(id, form as OrganizationUnitUpdateDto)
      : this.service.create(form);

    const message = 'AbpUi::SavedSuccessfully';

    this.isModalBusy = true;
    request.pipe(finalize(() => (this.isModalBusy = false))).subscribe(() => {
      this.get();
      this.isNodeModalVisible = false;
      this.toasterService.success(message);
    });
  }

  delete(selected: OrganizationUnitWithDetailsDto) {
    this.deleteOrganizationUnitService.openDeleteModal(selected);
  }

  getParentName(parentId: string) {
    const parent = this.organizationUnits.find(unit => unit.id === parentId);

    if (!parent) return '';

    return parent.displayName;
  }

  onDrop(event: DropEvent) {
    if (!event.node) return;

    let parentId = event.node.key;
    if (!event.node.origin.parentId && event.pos === -1) {
      parentId = null;
    }

    this.move(event.dragNode.key, parentId);
  }

  move(id: string, newParentId: string) {
    this.service.move(id, { newParentId }).subscribe(this.get);
  }

  setSelectedUnit(value: OrganizationUnitWithDetailsDto | undefined) {
    this.organizationUnitsStateService.setSelectedUnit(value);
  }

  onOUDeleted() {
    this.toasterService.success('AbpUi::DeletedSuccessfully');
    this.organizationUnitsStateService.setSelectedUnit(undefined);
    this.get();
  }

  onOUMoved() {
    this.get();
    this.organizationUnitsStateService.refreshSelectedNode();
  }

  moveAllUsers(selected: OrganizationUnitWithDetailsDto) {
    this.moveAllUsersOfUnitModalService.openMoveModal(selected);
  }
}
