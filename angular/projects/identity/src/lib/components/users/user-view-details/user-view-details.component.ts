import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  inject,
} from '@angular/core';
import { Observable } from 'rxjs';
import { NgbNavModule } from '@ng-bootstrap/ng-bootstrap';
import { TreeModule } from '@abp/ng.components/tree';
import { CoreModule } from '@abp/ng.core';
import { ThemeSharedModule } from '@abp/ng.theme.shared';
import { IdentityUserDto } from '@volo/abp.ng.identity/proxy';
import { OrganizationTreeModel } from '../../../models';
import { UserViewDetailService } from '../../../services';

@Component({
  standalone: true,
  selector: 'abp-user-view-detail',
  templateUrl: './user-view-details.component.html',
  imports: [NgbNavModule, TreeModule, CoreModule, ThemeSharedModule],
  providers: [UserViewDetailService],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserViewDetailsComponent implements OnInit {
  protected readonly service = inject(UserViewDetailService);

  @Input() user: IdentityUserDto;
  @Output() isModalVisibleChange = new EventEmitter<boolean>();
  isModalVisible = true;

  modifiedBy$: Observable<IdentityUserDto>;
  createdBy$: Observable<IdentityUserDto>;
  organization$: Observable<OrganizationTreeModel>;

  ngOnInit(): void {
    this.modifiedBy$ = this.service.getUserById(this.user.lastModifierId);
    this.createdBy$ = this.service.getUserById(this.user.creatorId);
    this.organization$ = this.service.getOrganizationUnits(this.user.id);
  }
}
